namespace Application.Products;
public class ProductList
{
    public class Query : IRequest<Result<List<ProductDetailDto>>>
    {

        public class Handler(DataContext context, IMapper mapper) : IRequestHandler<Query, Result<List<ProductDetailDto>>>
        {
            private readonly DataContext _context = context;
            private readonly IMapper _mapper = mapper;

            public async Task<Result<List<ProductDetailDto>>> Handle(Query request, CancellationToken ct)
            {
                var productList = await _context.Products
                    .Include(x => x.ProductName)
                    .Include(x => x.Location)
                    .ProjectTo<ProductDetailDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(ct);

                return Result<List<ProductDetailDto>>.Success(productList);
            }
        }
    }

}
