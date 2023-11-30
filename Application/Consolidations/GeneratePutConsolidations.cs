using Application.Core;
using Application.Extensions;
using Application.Products;
using Domain;

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

        public async Task<Result<List<ConsolidationPutDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var dbProducts = await _context.Products
             .Include(x => x.Location)
             .Include(x => x.PartNumber)
             .Where(x => x.PartNumber.Name.Contains(request.SearchParams.PartNumberName)
              && x.Location.Products.Count >= request.SearchParams.MaxUnit)
             .ToListAsync(cancellationToken: cancellationToken);

            var locations = dbProducts.Select(x => x.Location).Distinct().ToList();

            var output = new List<ConsolidationPutDto>();

            foreach (var location in locations)
            {
                var dbTotalLocationProducts = await _context.Products
                    .Include(x => x.Location)
                    .Where(x => x.Location == location)
                    .ToListAsync(cancellationToken: cancellationToken);

                int capacity = location.TotalCapacity == 0 ? 100 : location.TotalCapacity;

                //var emptyLocation = await _context.Locations
                //    .Include(x => x.Products)
                //    .Where(x => x.TotalCapacity==100)
                //    .ToListAsync(cancellationToken: cancellationToken);

                output.Add(new()
                {
                    Location = location.Name,
                    EneteredPartNumberTotalCount = dbProducts.Where(x => x.Location == location).Count(),
                    EmptySpace = capacity - dbTotalLocationProducts.Count,
                    //EmptyLocation = emptyLocation,
                });

            }
            return Result<List<ConsolidationPutDto>>.Success(output);
        }
    }

}
