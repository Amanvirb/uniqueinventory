namespace Application.Locations;
public class ProductNumberNameList
{
    public class Query : IRequest<Result<List<ProductNumberDto>>>
    {
        public class Handler(DataContext context, IMapper mapper) : IRequestHandler<Query, Result<List<ProductNumberDto>>>
        {
            private readonly DataContext _context = context;
            private readonly IMapper _mapper = mapper;

            public async Task<Result<List<ProductNumberDto>>> Handle(Query request, CancellationToken ct)
            {
                var productNumbers = await _context.ProductNumbers
                    .Include(x => x.Products)
                    .ProjectTo<ProductNumberDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(ct);

                return Result<List<ProductNumberDto>>.Success(productNumbers);

            }
        }
    }
}
