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

            var product = await _context.Products
                .Include(x => x.Location)
                .Include(x => x.PartNumberName)
                .FirstOrDefaultAsync(x => x.Id == request.UpdatedProduct.Id,
                cancellationToken: cancellationToken);

            if (product is null) return null;

            if (product.Location.LocationName == request.UpdatedProduct.LocationName && product.PartNumberName.PartNumberName == request.UpdatedProduct.PartNumberName) return Result<Unit>.Failure("Entered part number and location Name is same as previous"); ;

            var existingLocation = await _context.Locations
            .FirstOrDefaultAsync(x => x.LocationName == request.UpdatedProduct.LocationName, cancellationToken: cancellationToken);
            
            if (existingLocation is null) return null;

            var existingPartNumber = await _context.PartNumbers
                .FirstOrDefaultAsync(x => x.PartNumberName == request.UpdatedProduct.PartNumberName, cancellationToken: cancellationToken);

            if (existingPartNumber is null) return null;

            var updatedProduct = new ProductUpdateHistory
            {
                SerialNumber = product.SerialNumber,
                Location = product.Location.LocationName != request.UpdatedProduct.LocationName ? existingLocation.LocationName : product.Location.LocationName,
                PartNumber = product.PartNumberName.PartNumberName!= request.UpdatedProduct.PartNumberName ? existingPartNumber.PartNumberName : product.PartNumberName.PartNumberName,
                DateTime = DateTime.Now,
            };

            product.Location = existingLocation;

            product.PartNumberName = existingPartNumber;

            _context.Entry(product).State = EntityState.Modified;

            _context.ProductUpdateHistories.Add(updatedProduct);

            result = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (!result) return Result<Unit>.Failure("Failed to update Product Detail");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
