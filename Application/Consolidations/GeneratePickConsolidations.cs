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

        public async Task<Result<List<ConsolidationPickDto>>> Handle(Query request, CancellationToken ct)
        {
            var productName = request.SearchParams.ProductNumberName.Trim().ToUpper();

            var dbProducts = await _context.Products
             .Include(x => x.Location)
             .Include(x => x.ProductNumber)
             .Where(x => x.ProductNumber.Name.Contains(productName)
                 && x.Location.Products.Count <= request.SearchParams.MaxUnit)
             .ToListAsync();

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
                    ProductNumberName = productName,
                });

            }
            return Result<List<ConsolidationPickDto>>.Success(output);

        }
    }

}
