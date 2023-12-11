namespace Application.Products;

public class EditProduct
{
    public class Command : IRequest<Result<Unit>>
    {
        public UpdateProductDto UpdatedProduct { get; set; }
    }
    public class Handler(DataContext context, IMapper mapper) : IRequestHandler<Command, Result<Unit>>
    {
        private DataContext _context = context;
        private IMapper _mapper = mapper;

        public async Task<Result<Unit>> Handle(Command request, CancellationToken ct)
        {
            bool result;

            var updatedProduct = request.UpdatedProduct;

            updatedProduct.LocationName = updatedProduct.LocationName.Trim().ToUpper();
            updatedProduct.ProductNumberName = updatedProduct.ProductNumberName.Trim().ToUpper();

            var product = await _context.Products
                .Include(x => x.Location)
                .Include(x => x.ProductNumber)
                .FirstOrDefaultAsync(x => x.Id == request.UpdatedProduct.Id,
                ct);

            if (product is null) return null;

            if (product.Location.Name == updatedProduct.LocationName
                && product.ProductNumber.Name == updatedProduct.ProductNumberName)
                return Result<Unit>.Failure("Entered product name and location Name is same as previous");

            var existingLocation = await _context.Locations
            .FirstOrDefaultAsync(x => x.Name == request.UpdatedProduct.LocationName, ct);

            if (existingLocation is null) return null;

            var existingProductNumber = await _context.ProductNumbers
                .FirstOrDefaultAsync(x => x.Name == request.UpdatedProduct.ProductNumberName, ct);

            if (existingProductNumber is null) return null;

            var updatedNewProduct = new ProductUpdateHistory
            {
                SerialNumber = product.SerialNumber,
                Location = product.Location.Name != updatedProduct.LocationName ? existingLocation.Name : product.Location.Name,
                ProductNumber = product.ProductNumber.Name != updatedProduct.ProductNumberName ? existingProductNumber.Name : product.ProductNumber.Name,
                DateTime = DateTime.Now,
            };

            product.Location = existingLocation;

            product.ProductNumber = existingProductNumber;

            _context.Entry(product).State = EntityState.Modified;

            //await _context.Products
            //    .Where(x => x.Id == updatedNewProduct.Id)
            //    .ExecuteUpdateAsync(x => x.SetProperty(x => x.Location, existingLocation), ct);

            _context.ProductUpdateHistories.Add(updatedNewProduct);

            result = await _context.SaveChangesAsync(ct) > 0;
            if (!result) return Result<Unit>.Failure("Failed to update Product Detail");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
