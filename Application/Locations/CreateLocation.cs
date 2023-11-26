﻿

namespace Application.Locations;
public class CreateLocation
{
public class Command : IRequest<Result<Unit>>
    {
        public CommonDto Location { get; set; }
    }
    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var existingLocation = await _context.Locations.FirstOrDefaultAsync(x => x.LocationName == request.Location.Name,
                cancellationToken: cancellationToken);
            if (existingLocation is not null) return Result<Unit>.Failure("Location already exists, Please Add new location Name");

            var location = new Location
            {
                LocationName = request.Location.Name,
            };

            _context.Locations.Add(location);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (!result) Result<Unit>.Failure("Can not create location");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
