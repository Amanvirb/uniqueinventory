
namespace Application.Products;

public class ProductDetail
{
    public class Query : IRequest<Result<ProductDto>>
    {
        public int Id { get; set; }
    }
    public class Handler(DataContext context, IMapper mapper) : IRequestHandler<Query, Result<ProductDto>>
    {
        private DataContext _context = context;
        private IMapper _mapper = mapper;

        public async Task<Result<ProductDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
            
            if (product is null) return Result<ProductDto>.Failure("Product not found");

            return Result<ProductDto>.Success(product);
        }
    }
}
