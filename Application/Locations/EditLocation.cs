﻿using FluentValidation;

namespace Application.Products;

public class EditLocation
{
    public class Command : IRequest<Result<Unit>>
    {
        public CommonDto Location { get; set; }
        public int Id { get; set; }
    }
    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Location).NotEmpty();
        }
    }
    public class Handler(DataContext context) : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context = context;

        public async Task<Result<Unit>> Handle(Command request, CancellationToken ct)
        {
            bool result;

            var updatedLocation = request.Location.Name.Trim().ToUpper();

            var dbLocation = await _context.Locations
                .Include(x => x.Products)
                .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

            if (dbLocation is null) return null;

            if (dbLocation.Name == updatedLocation)
                return Result<Unit>.Failure("Entered location name is same as previous");

            result = await _context.Locations
                .ExecuteUpdateAsync(x => x.SetProperty(p => p.Name, updatedLocation), ct) > 0;

            if (!result) return Result<Unit>.Failure("Failed to update location");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}