namespace Application.Products;

public class AddProduct
{
    public class Command : IRequest<Result<Unit>>
    {
        public ProductDto Product { get; set; }
    }
    public class Handler(DataContext context) : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context = context;

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            bool result;

            var existingProduct = await _context.Products
                .Include(x=>x.Location)
                .FirstOrDefaultAsync(x => x.SerialNumber == request.Product.SerialNumber,
                cancellationToken: cancellationToken);
            if (existingProduct is not null)
            {
                var previousProduct = new SerialNumberHistory
                {
                    SerialNumber = existingProduct.SerialNumber,
                    LocationName = existingProduct.Location.LocationName,
                    PartNumberName = existingProduct.PartNumberName,
                    DateTime = DateTime.Now,
                };
                _context.SerialNumberHistories.Add(previousProduct);
            }

            var location = await _context.Locations.FirstOrDefaultAsync(x => x.LocationName == request.Product.LocationName,
                cancellationToken: cancellationToken);

            if (location is null)
            {
                var newLocation = new Location
                {
                    LocationName = request.Product.LocationName,
                };

                _context.Locations.Add(newLocation);
                result = await _context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Can not create Location");

            }

            var partNumber = await _context.PartNumbers.FirstOrDefaultAsync(x => x.PartNumberName == request.Product.PartNumberName,
                cancellationToken: cancellationToken);

            if (partNumber is null)
            {
                var newPartNumber = new PartNumber
                {
                    PartNumberName = request.Product.PartNumberName,
                };

                _context.PartNumbers.Add(newPartNumber);
                result = await _context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to add Part Number");
            }

            var requestedLocation = new Location
            {
                LocationName = request.Product.LocationName,
            };

            var newProduct = new Product
            {
                SerialNumber = request.Product.SerialNumber,
                PartNumberName = request.Product.PartNumberName,
                Location = requestedLocation,
            };
            _context.Products.Add(newProduct);

            result = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (!result) return Result<Unit>.Failure("Failed to add Product");

            return Result<Unit>.Success(Unit.Value);
        }

    }
}
