using Application.Core.UtilHelper;

namespace Application.Consolidations;
public class GeneratePickConsolidations
{
    public class Query : IRequest<Result<List<ConsolidationPickDto>>>
    {
        public SearchParams SearchParams { get; set; }
    }
    public class Handler(DataContext context) : IRequestHandler<Query, Result<List<ConsolidationPickDto>>>
    {
        private readonly DataContext _context = context;

        public async Task<Result<List<ConsolidationPickDto>>> Handle(Query request, CancellationToken ct)
        {
            var productName = request.SearchParams.ProductName;

            var dbProducts = await _context.Products
             .Include(x => x.Location)
             .Include(x => x.ProductName)
             .Where(x => x.ProductName.Name.Contains(productName)
                 && x.Location.Products.Count <= request.SearchParams.MaxUnit)
             .ToListAsync(ct);

            var output = UtilHelper.GenConsolidatePick(productName, dbProducts);

            return Result<List<ConsolidationPickDto>>.Success(output);

        }
    }

}
