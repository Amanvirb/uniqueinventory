using Application.Locations;
using FluentValidation;

namespace Application.Products;

public class EditLocation
{
    public class Command : IRequest<Result<Unit>>
    {
        public CommonDto Location { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Location).SetValidator(new CommonValidator());
        }
    }
    public class Handler(DataContext context) : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context = context;

        public async Task<Result<Unit>> Handle(Command request, CancellationToken ct)
        {
            bool result;

            var updatedLocation = request.Location.Name;

            var dbLocation = await _context.Locations
                .Include(x => x.Products)
                .FirstOrDefaultAsync(x => x.Id == request.Location.Id, ct);

            if (dbLocation is null) return Result<Unit>.Failure("Location does not exist");

            if (dbLocation.Name == updatedLocation)
                return Result<Unit>.Failure("Entered location name is same as previous");

            result = await _context.Locations
                .Where(x => x.Id == request.Location.Id)
                .ExecuteUpdateAsync(x => x.SetProperty(p => p.Name, updatedLocation), ct) > 0;

            if (!result) return Result<Unit>.Failure("Failed to update location");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
