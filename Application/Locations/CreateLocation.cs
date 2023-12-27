using Application.Products;
using FluentValidation;
using static Application.Products.AddProduct;

namespace Application.Locations;
public class CreateLocation
{
    public class Command : IRequest<Result<Unit>>
    {
        public AddLocationDto Location { get; set; }
    }
    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Location).SetValidator(new CreateLocationValidator());
        }
    }
    public class Handler(DataContext context) : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context = context;

        public async Task<Result<Unit>> Handle(Command request, CancellationToken ct)
        {
            var existingLocation = await _context.Locations.FirstOrDefaultAsync(x => x.Name == request.Location.Name, ct);

            if (existingLocation is not null) return Result<Unit>.Failure("Location already exists");

            var location = new Location
            {
                Name = request.Location.Name.Trim().ToUpper(),
                TotalCapacity = request.Location.TotalCapacity,
            };

            _context.Locations.Add(location);

            var result = await _context.SaveChangesAsync(ct) > 0;
            if (!result) Result<Unit>.Failure("Can not create location");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
