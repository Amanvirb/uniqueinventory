using Domain;
using FluentValidation;

namespace Application.Products;

public class AddProducts
{
    public class AddProductsCommand : IRequest<Result<Unit>>
    {
        public ScannedQRDto Product { get; set; }

    }

    public class CommandValidator : AbstractValidator<AddProductsCommand>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Product).SetValidator(new ScannedProductValidator());
        }
    }

    public class Handler(DataContext context) : IRequestHandler<AddProductsCommand, Result<Unit>>
    {
        private readonly DataContext _context = context;

        public async Task<Result<Unit>> Handle(AddProductsCommand request, CancellationToken ct)
        {
            bool result;

            var newProduct = request.Product;

            newProduct.LocationName = newProduct.LocationName.Trim().ToUpper();

            var location = await _context.Locations.FirstOrDefaultAsync(x => x.Name == newProduct.LocationName, ct);

            if (location is null)
            {
                location = new Location
                {
                    Name = newProduct.LocationName,
                };
                _context.Locations.Add(location);
                result = await _context.SaveChangesAsync(ct) > 0;
                if (!result) return Result<Unit>.Failure("Can not create Location");
            }

            List<ProductName> dbProductNames = [];

            foreach (var pname in newProduct.Products.Select(x => x.ProductName).Distinct())
            {
                ProductName productName = await _context.ProductNames.FirstOrDefaultAsync(x => x.Name == pname, ct);

                if (productName is null)
                {
                    productName = new ProductName
                    {
                        Name = pname,
                    };
                    _context.ProductNames.Add(productName);
                    result = await _context.SaveChangesAsync(ct) > 0;
                    if (!result) return Result<Unit>.Failure("Failed to add Product Name");
                }
                dbProductNames.Add(productName);
            }

            foreach (var product in newProduct.Products)
            {

                var existingProduct = await _context.Products
                   .Include(x => x.Location)
                   .Include(x => x.ProductName)
                   .FirstOrDefaultAsync(x => x.SerialNumber == product.SerialNo, ct);

                if (existingProduct is not null)
                {
                    var previousProduct = new SerialNumberHistory
                    {
                        SerialNumber = existingProduct.SerialNumber,
                        LocationName = existingProduct.Location.Name,
                        ProductName = existingProduct.ProductName.Name,
                        DateTime = DateTime.Now,
                    };
                    _context.SerialNumberHistories.Add(previousProduct);

                    existingProduct.Location = location;

                    _context.Entry(existingProduct).State = EntityState.Modified;
                    result = await _context.SaveChangesAsync(ct) > 0;
                    if (!result) return Result<Unit>.Failure("Failed to add Product in History");

                }
                else
                {
                    var dbNewProduct = new Product
                    {
                        SerialNumber = product.SerialNo,
                        ProductName = dbProductNames.FirstOrDefault(x => x.Name == product.ProductName),
                        Location = location,
                    };

                    _context.Products.Add(dbNewProduct);
                    result = await _context.SaveChangesAsync(ct) > 0;
                    if (!result) return Result<Unit>.Failure("Failed to add Product");

                }

            }

            return Result<Unit>.Success(Unit.Value);
        }

    }
}
