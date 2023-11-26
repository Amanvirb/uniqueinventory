namespace Application.Products;

public class EditProduct
{
    public class Command : IRequest<Result<Unit>>
    {
        public UpdateProductDto updatedProduct { get; set; }
    }
    public class Handler(DataContext context, IMapper mapper) : IRequestHandler<Command, Result<Unit>>
    {
        private DataContext _context = context;
        private IMapper _mapper = mapper;


        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == request.updatedProduct.Id,
                cancellationToken: cancellationToken);
            if (product is null) return null;

            if (product.Location.LocationName == request.updatedProduct.LocationName && product.PartNumberName == request.updatedProduct.PartNumberName) return null;

            if (product.Location.LocationName != request.updatedProduct.LocationName)
            {
                var updatedProduct = new ProductUpdateHistory
                {
                    SerialNumber = product.SerialNumber,
                    Location = request.updatedProduct.LocationName,
                    PartNumber = product.PartNumberName,
                    DateTime = DateTime.Now,
                };

                product.Location.LocationName = request.updatedProduct.LocationName;

                _context.Entry(product).State = EntityState.Modified;
                _context.ProductUpdateHistories.Add(updatedProduct);

            }
            if (product.PartNumberName != request.updatedProduct.PartNumberName)
            {
                var updatedProduct = new ProductUpdateHistory
                {
                    SerialNumber = product.SerialNumber,
                    Location = product.Location.LocationName,
                    PartNumber = request.updatedProduct.PartNumberName,
                    DateTime = DateTime.Now,

                };
                product.PartNumberName = request.updatedProduct.PartNumberName;

                _context.Entry(product).State = EntityState.Modified;
                _context.ProductUpdateHistories.Add(updatedProduct);
            }

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (!result) return Result<Unit>.Failure("Failed to update Product Detail");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
