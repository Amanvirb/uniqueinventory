using FluentValidation;

namespace Application.Products;

public class EditProduct
{
    public class Command : IRequest<Result<Unit>>
    {
        public UpdateProductDto UpdatedProduct { get; set; }
    }
    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.UpdatedProduct).SetValidator(new UpdateProductValidator());
        }
    }
    public class Handler(DataContext context) : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context = context;

        public async Task<Result<Unit>> Handle(Command request, CancellationToken ct)
        {
            bool result;

            var updatedProduct = request.UpdatedProduct;

            var dbProduct = await _context.Products
                .Include(x => x.Location)
                .Include(x => x.ProductNumber)
                .FirstOrDefaultAsync(x => x.Id == updatedProduct.Id, ct);

            if (dbProduct is null) return Result<Unit>.Failure("Product not found");

            if (dbProduct.Location.Name == updatedProduct.LocationName
                && dbProduct.ProductNumber.Name == updatedProduct.ProductName)
                return Result<Unit>.Failure("Entered product name and location Name is same as previous");

            var existingLocation = await _context.Locations
            .FirstOrDefaultAsync(x => x.Name == updatedProduct.LocationName, ct);

            if (existingLocation is null) return Result<Unit>.Failure("Location does not exist");

            var existingProductName = await _context.ProductNumbers
                .FirstOrDefaultAsync(x => x.Name == updatedProduct.ProductName, ct);

            if (existingProductName is null) return Result<Unit>.Failure("Product name does not exist");

            var updatedNewProduct = new ProductUpdateHistory
            {
                SerialNumber = dbProduct.SerialNumber,
                Location = dbProduct.Location.Name != updatedProduct.LocationName ? existingLocation.Name : dbProduct.Location.Name,
                ProductNumber = dbProduct.ProductNumber.Name != updatedProduct.ProductName ? existingProductName.Name : dbProduct.ProductNumber.Name,
                DateTime = DateTime.Now,
            };

            dbProduct.Location = existingLocation;

            dbProduct.ProductNumber = existingProductName;

            _context.Entry(dbProduct).State = EntityState.Modified;

            _context.ProductUpdateHistories.Add(updatedNewProduct);

            result = await _context.SaveChangesAsync(ct) > 0;
            if (!result) return Result<Unit>.Failure("Failed to update Product Detail");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
