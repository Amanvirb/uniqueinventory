namespace Application.ProductNames;
public class ProductNameList
{
    public class Query : IRequest<Result<List<ProductNameDto>>>
    {
        public class Handler(DataContext context, IMapper mapper) : IRequestHandler<Query, Result<List<ProductNameDto>>>
        {
            private readonly DataContext _context = context;
            private readonly IMapper _mapper = mapper;

            public async Task<Result<List<ProductNameDto>>> Handle(Query request, CancellationToken ct)
            {
                var ProductNames = await _context.ProductNames
                    .Include(x => x.Products)
                    .ProjectTo<ProductNameDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(ct);

                return Result<List<ProductNameDto>>.Success(ProductNames);

            }
        }
    }
}
