using Application.Core;
using Application.Extensions;
using Application.Products;
using Domain;
using Microsoft.Extensions.Logging;

namespace Application.Consolidations;
public class GeneratePutConsolidations
{
    public class Query : IRequest<Result<List<ConsolidationPutDto>>>
    {
        public SearchParams SearchParams { get; set; }
    }
    public class Handler(DataContext context, IMapper mapper) : IRequestHandler<Query, Result<List<ConsolidationPutDto>>>
    {
        private readonly DataContext _context = context;
        private readonly IMapper _mapper = mapper;
        //private readonly ILogger<GeneratePutConsolidations> _logger = logger;

        public async Task<Result<List<ConsolidationPutDto>>> Handle(Query request, CancellationToken ct)
        {
            var dbProducts = await _context.Products
             .Include(x => x.Location)
             .Include(x => x.ProductNumber)
             .Where(x => x.ProductNumber.Name.Contains(request.SearchParams.ProductNumberName)
              && x.Location.Products.Count >= request.SearchParams.MaxUnit)
             .ToListAsync(ct);

            var locations = dbProducts.Select(x => x.Location).Distinct().ToList();

            //var testingVar = "TEsting variable";

            //_logger.LogInformation(testingVar);

            var output = new List<ConsolidationPutDto>();

            foreach (var location in locations)
            {
                var dbTotalLocationProducts = await _context.Products
                    .Include(x => x.Location)
                    .Where(x => x.Location == location)
                    .ToListAsync(ct);

                int capacity = location.TotalCapacity == 0 ? 100 : location.TotalCapacity;

                //var emptyLocation = await _context.Locations
                //    .Include(x => x.Products)
                //    .Where(x => x.TotalCapacity==100)
                //    .ToListAsync(cancellationToken: cancellationToken);

                output.Add(new()
                {
                    Location = location.Name,
                    EneteredProductNumberTotalCount = dbProducts.Where(x => x.Location == location).Count(),
                    EmptySpace = capacity - dbTotalLocationProducts.Count,
                });

            }

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

            //var emptyLocations = await _context.Locations
            //    .Include(x => x.Products)
            //    .Where(x => x.Products.Count == 0)
            //    .ToListAsync(cancellationToken: cancellationToken);

            //foreach (var elocation in emptyLocations)
            //{
            //    output.Add(new()
            //    {
            //        Location = elocation.Name,
            //        EmptySpace = elocation.TotalCapacity,
            //    });
            //}

            return Result<List<ConsolidationPutDto>>.Success(output);
        }
    }

}
