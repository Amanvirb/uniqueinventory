namespace Application.Consolidations;
public class GeneratePutConsolidations
{
    public class Query : IRequest<Result<List<ConsolidationPutDto>>>
    {
        public SearchParams SearchParams { get; set; }
    }
    public class Handler(DataContext context) : IRequestHandler<Query, Result<List<ConsolidationPutDto>>>
    {
        private readonly DataContext _context = context;
        public async Task<Result<List<ConsolidationPutDto>>> Handle(Query request, CancellationToken ct)
        {
            var productName = request.SearchParams.ProductName;

            var dbProducts = await _context.Products
             .Include(x => x.Location)
             .Include(x => x.ProductName)
             .Where(x => x.ProductName.Name.Contains(productName)
              && x.Location.Products.Count >= request.SearchParams.MaxUnit)
             .ToListAsync(ct);

            var locations = dbProducts.Select(x => x.Location).Distinct().ToList();

            var output = new List<ConsolidationPutDto>();

            foreach (var location in locations)
            {
                var dbTotalLocationProducts = await _context.Products
                    .Include(x => x.Location)
                    .Where(x => x.Location == location)
                    .ToListAsync(ct);

                int capacity = location.TotalCapacity == 0 ? 100 : location.TotalCapacity;

                output.Add(new()
                {
                    Location = location.Name,
                    EneteredProductNameTotalCount = dbProducts.Where(x => x.Location == location).Count(),
                    EmptySpace = capacity - dbTotalLocationProducts.Count,
                });

            }

            //Get all empty Locations
            var emptyLocations = await _context.Locations
                .Include(x => x.Products)
                .Where(x => x.Products.Count == 0)
                .Select(l => new ConsolidationPutDto
                {
                    Location = l.Name,
                    EmptySpace = l.TotalCapacity,
                })
                .ToListAsync(ct);

            output.AddRange(emptyLocations);

            return Result<List<ConsolidationPutDto>>.Success(output);
        }
    }

}
