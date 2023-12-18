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
        private DataContext _context = context;

        public async Task<Result<Unit>> Handle(Command request, CancellationToken ct)
        {
            bool result;

            var updatedProduct = request.UpdatedProduct;

            updatedProduct.LocationName = updatedProduct.LocationName.Trim().ToUpper();
            updatedProduct.ProductName = updatedProduct.ProductName.Trim().ToUpper();

            var dbProduct = await _context.Products
                .Include(x => x.Location)
                .Include(x => x.ProductNumber)
                .FirstOrDefaultAsync(x => x.Id == request.UpdatedProduct.Id, ct);

            if (dbProduct is null) return null;

            if (dbProduct.Location.Name == updatedProduct.LocationName
                && dbProduct.ProductNumber.Name == updatedProduct.ProductName)
                return Result<Unit>.Failure("Entered product name and location Name is same as previous");

            var existingLocation = await _context.Locations
            .FirstOrDefaultAsync(x => x.Name == request.UpdatedProduct.LocationName, ct);

            if (existingLocation is null) return null;

            var existingProductNumber = await _context.ProductNumbers
                .FirstOrDefaultAsync(x => x.Name == request.UpdatedProduct.ProductName, ct);

            if (existingProductNumber is null) return null;

            var updatedNewProduct = new ProductUpdateHistory
            {
                SerialNumber = dbProduct.SerialNumber,
                Location = dbProduct.Location.Name != updatedProduct.LocationName ? existingLocation.Name : dbProduct.Location.Name,
                ProductNumber = dbProduct.ProductNumber.Name != updatedProduct.ProductName ? existingProductNumber.Name : dbProduct.ProductNumber.Name,
                DateTime = DateTime.Now,
            };

            dbProduct.Location = existingLocation;

            dbProduct.ProductNumber = existingProductNumber;

            _context.Entry(dbProduct).State = EntityState.Modified;


            _context.ProductUpdateHistories.Add(updatedNewProduct);

            result = await _context.SaveChangesAsync(ct) > 0;
            if (!result) return Result<Unit>.Failure("Failed to update Product Detail");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
