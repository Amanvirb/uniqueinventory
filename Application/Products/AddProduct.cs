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
            var newProduct = request.Product;
            newProduct.LocationName = newProduct.LocationName.Trim().ToUpper();
            newProduct.ProductNumberName = newProduct.ProductNumberName.Trim().ToUpper();
            newProduct.SerialNumber = newProduct.SerialNumber.Trim().ToUpper();

            var location = await _context.Locations.FirstOrDefaultAsync(x => x.Name == newProduct.LocationName,
              cancellationToken: cancellationToken);

            if (location is null)
            {
                location = new Location
                {
                    Name = newProduct.LocationName,
                };
                _context.Locations.Add(location);
                result = await _context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Can not create Location");
            }

            var partNumber = await _context.ProductNumbers.FirstOrDefaultAsync(x => x.Name == newProduct.ProductNumberName,
                cancellationToken: cancellationToken);

            if (partNumber is null)
            {
                partNumber = new ProductNumber
                {
                    Name = newProduct.ProductNumberName,
                };
                _context.ProductNumbers.Add(partNumber);
                result = await _context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to add Part Number");
            }

            var existingProduct = await _context.Products
               .Include(x => x.Location)
               .Include(x => x.ProductNumber)
               .FirstOrDefaultAsync(x => x.SerialNumber == newProduct.SerialNumber,
               cancellationToken: cancellationToken);

            if (existingProduct is not null)
            {
                var previousProduct = new SerialNumberHistory
                {
                    SerialNumber = existingProduct.SerialNumber.Trim().ToUpper(),
                    LocationName = existingProduct.Location.Name.Trim().ToUpper(),
                    ProductNumberName = existingProduct.ProductNumber.Name.Trim().ToUpper(),
                    DateTime = DateTime.Now,
                };
                _context.SerialNumberHistories.Add(previousProduct);
            }

            var dbNewProduct = new Product
            {
                SerialNumber = newProduct.SerialNumber,
                ProductNumber = partNumber,
                Location = location,
            };

            _context.Products.Add(dbNewProduct);

            result = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (!result) return Result<Unit>.Failure("Failed to add Product");

            return Result<Unit>.Success(Unit.Value);
        }

    }
}
