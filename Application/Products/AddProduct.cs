using FluentValidation;

namespace Application.Products;

public class AddProduct
{
    public class AddProductCommand : IRequest<Result<Unit>>
    {
        public ProductDto Product { get; set; }
    }

    public class CommandValidator : AbstractValidator<AddProductCommand>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Product).SetValidator(new ProductValidator());
        }
    }

    public class Handler(DataContext context) : IRequestHandler<AddProductCommand, Result<Unit>>
    {
        private readonly DataContext _context = context;

        public async Task<Result<Unit>> Handle(AddProductCommand request, CancellationToken ct)
        {
            bool result;
            var newProduct = request.Product;

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

            var ProductName = await _context.ProductNames.FirstOrDefaultAsync(x => x.Name == newProduct.ProductName,
               ct);

            if (ProductName is null)
            {
                ProductName = new ProductName
                {
                    Name = newProduct.ProductName,
                };
                _context.ProductNames.Add(ProductName);
                result = await _context.SaveChangesAsync(ct) > 0;
                if (!result) return Result<Unit>.Failure("Failed to add Product");
            }

            var existingProduct = await _context.Products
               .Include(x => x.Location)
               .Include(x => x.ProductName)
               .FirstOrDefaultAsync(x => x.SerialNumber == newProduct.SerialNumber, ct);

            if (existingProduct is not null)
            {
                var previousProduct = new SerialNumberHistory
                {
                    SerialNumber = existingProduct.SerialNumber,
                    LocationName = existingProduct.Location.Name,
                    ProductName = existingProduct.ProductName.Name,
                    Remarks = "Serisl number repeated",
                    DateTime = DateTime.Now,
                };
                _context.SerialNumberHistories.Add(previousProduct);
                //_context.ProductUpdateHistories.Add(previousProduct);


                existingProduct.ProductName = ProductName;
                existingProduct.Location = location;

                _context.Entry(existingProduct).State = EntityState.Modified;
            }
            else
            {
                var dbNewProduct = new Product
                {
                    SerialNumber = newProduct.SerialNumber,
                    ProductName = ProductName,
                    Location = location,
                };

                _context.Products.Add(dbNewProduct);
            }

            result = await _context.SaveChangesAsync(ct) > 0;
            if (!result) return Result<Unit>.Failure("Failed to add Product");

            return Result<Unit>.Success(Unit.Value);
        }

    }
}
