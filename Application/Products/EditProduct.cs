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

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            bool result;

            var updatedProduct = request.UpdatedProduct;

            updatedProduct.LocationName = updatedProduct.LocationName.Trim().ToUpper();
            updatedProduct.PartNumberName = updatedProduct.PartNumberName.Trim().ToUpper();

            var product = await _context.Products
                .Include(x => x.Location)
                .Include(x => x.PartNumber)
                .FirstOrDefaultAsync(x => x.Id == request.UpdatedProduct.Id,
                cancellationToken: cancellationToken);

            if (product is null) return null;

            if (product.Location.Name == updatedProduct.LocationName
                && product.PartNumber.Name == updatedProduct.PartNumberName)
                return Result<Unit>.Failure("Entered part number and location Name is same as previous");

            var existingLocation = await _context.Locations
            .FirstOrDefaultAsync(x => x.Name == request.UpdatedProduct.LocationName, cancellationToken: cancellationToken);

            if (existingLocation is null) return null;

            var existingPartNumber = await _context.ProductNumbers
                .FirstOrDefaultAsync(x => x.Name == request.UpdatedProduct.PartNumberName, cancellationToken: cancellationToken);

            if (existingPartNumber is null) return null;

            var updatedNewProduct = new ProductUpdateHistory
            {
                SerialNumber = product.SerialNumber,
                Location = product.Location.Name != updatedProduct.LocationName ? existingLocation.Name : product.Location.Name,
                PartNumber = product.PartNumber.Name != updatedProduct.PartNumberName ? existingPartNumber.Name : product.PartNumber.Name,
                DateTime = DateTime.Now,
            };

            product.Location = existingLocation;

            product.PartNumber = existingPartNumber;

            _context.Entry(product).State = EntityState.Modified;

            _context.ProductUpdateHistories.Add(updatedNewProduct);

            result = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (!result) return Result<Unit>.Failure("Failed to update Product Detail");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
