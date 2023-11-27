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
            var product = await _context.Products
                .Include(x=>x.Location)
                .FirstOrDefaultAsync(x => x.Id == request.UpdatedProduct.Id,
                cancellationToken: cancellationToken);
            if (product is null) return null;

            if (product.Location.LocationName == request.UpdatedProduct.LocationName && product.PartNumberName == request.UpdatedProduct.PartNumberName) return null;


            if (product.Location.LocationName != request.UpdatedProduct.LocationName)
            {
                var updatedProduct = new ProductUpdateHistory
                {
                    SerialNumber = product.SerialNumber,
                    Location = request.UpdatedProduct.LocationName,
                    PartNumber = product.PartNumberName,
                    DateTime = DateTime.Now,
                };

                product.Location.LocationName = request.UpdatedProduct.LocationName;

                _context.Entry(product).State = EntityState.Modified;
                _context.ProductUpdateHistories.Add(updatedProduct);

            }
            if (product.PartNumberName != request.UpdatedProduct.PartNumberName)
            {
                var updatedProduct = new ProductUpdateHistory
                {
                    SerialNumber = product.SerialNumber,
                    Location = product.Location.LocationName,
                    PartNumber = request.UpdatedProduct.PartNumberName,
                    DateTime = DateTime.Now,

                };
                product.PartNumberName = request.UpdatedProduct.PartNumberName;

                _context.Entry(product).State = EntityState.Modified;
                _context.ProductUpdateHistories.Add(updatedProduct);
            }

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (!result) return Result<Unit>.Failure("Failed to update Product Detail");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
