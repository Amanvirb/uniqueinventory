using Application.Core;
using Application.Extensions;
using Application.Products;
using Domain;

namespace Application.Consolidations;
public class GeneratePickConsolidations
{
    public class Query : IRequest<Result<List<ConsolidationPickDto>>>
    {
        public SearchParams SearchParams { get; set; }
    }
    public class Handler(DataContext context, IMapper mapper) : IRequestHandler<Query, Result<List<ConsolidationPickDto>>>
    {
        private readonly DataContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<List<ConsolidationPickDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var dbProducts = await _context.Products
             .Include(x => x.Location)
             .Include(x => x.ProductNumber)
             .Where(x => x.ProductNumber.Name.Contains(request.SearchParams.ProductNumberName)
                 && x.Location.Products.Count <= request.SearchParams.MaxUnit)
             .ToListAsync();

            //var dbProductsTest = await _context.Locations
            //.Include(x => x.Products)
            //.Where(x => x.Name.StartsWith("L") && x.Products.Any(p => p.SerialNumber.Contains("seriala")))
            //    .ToListAsync();


            var locations = dbProducts.Select(x => x.Location).Distinct().ToList();

            var output = new List<ConsolidationPickDto>();

            foreach (var location in locations)
            {
                var serials = dbProducts
                    .Where(x => x.Location == location)
                    .Select((p) => new ConsolidateSerialDto
                    {
                        SerialNo = p.SerialNumber
                    })
                    .ToList();

                output.Add(new()
                {
                    LocationName = location.Name,
                    Serials = serials,
                    ProductNumberName = request.SearchParams.ProductNumberName,
                });

            }
            return Result<List<ConsolidationPickDto>>.Success(output);

        }
    }

}
