namespace Application.Products;
public class ProductList
{
    public class Query : IRequest<Result<List<ProductDto>>>
    {
        public class Handler(DataContext context, IMapper mapper) : IRequestHandler<Query, Result<List<ProductDto>>>
        {
            private readonly DataContext _context = context;
            private readonly IMapper _mapper = mapper;

            public async Task<Result<List<ProductDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var productList = await _context.Products
                    .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken: cancellationToken);
                
                if (productList.Count < 0) return null;

                return Result<List<ProductDto>>.Success(productList);
            }
        }
    }
}
