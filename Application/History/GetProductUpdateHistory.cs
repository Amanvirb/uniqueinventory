
namespace Application.History;
public class GetProductUpdateHistory
{
    public class Query : IRequest<Result<List<ProductUpdateHistoryDto>>>
    {
        public ProductUpdateHistoryDto ProductUpdateHistory { get; set; }
    }

    public class Handler(DataContext context, IMapper mapper) : IRequestHandler<Query, Result<List<ProductUpdateHistoryDto>>>
    {
        private readonly DataContext _context = context;
        private readonly IMapper _mapper = mapper;


        public async Task<Result<List<ProductUpdateHistoryDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var productHistory = await _context.ProductUpdateHistories
                .ProjectTo<ProductUpdateHistoryDto>(_mapper.ConfigurationProvider)
                .Take(30)
                .ToListAsync(cancellationToken: cancellationToken);

            if (productHistory.Count < 0) return null;

            return Result<List<ProductUpdateHistoryDto>>.Success(productHistory);
         }

    }

}
