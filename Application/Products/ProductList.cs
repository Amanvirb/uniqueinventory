namespace Application.Products;
public class ProductList
{
    public class Query : IRequest<Result<List<ProductDto>>>
    {
        public class Handler(DataContext context, IMapper mapper) : IRequestHandler<Query, Result<List<ProductDto>>>
        {
            private readonly DataContext _context = context;
            private readonly IMapper _mapper = mapper;

            public async Task<Result<List<ProductDto>>> Handle(Query request, CancellationToken ct)
            {
                var productList = await _context.Products
                    .Include(x => x.Location)
                    .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(ct);

                return Result<List<ProductDto>>.Success(productList);
            }
        }
    }
}
