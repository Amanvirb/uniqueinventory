using Application.Core;
using Application.Extensions;
using Application.Products;
using Domain;

namespace Application.Consolidations;
public class GenerateConsolidations
{
    public class Query : IRequest<Result<List<ConsolidationDto>>>
    {
        public SearchParams SearchParams { get; set; }
    }
    public class Handler(DataContext context, IMapper mapper) : IRequestHandler<Query, Result<List<ConsolidationDto>>>
    {
        private readonly DataContext _context = context;
        private readonly IMapper _mapper = mapper;
        private object x;

        public async Task<Result<List<ConsolidationDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var dbProducts = await _context.Products
             .Include(x => x.Location)
             .Include(x => x.PartNumber)
             .Where(x => x.PartNumber.Name.Contains(request.SearchParams.PartNumberName)
                 && x.Location.Products.Count <= request.SearchParams.MaxUnit)
             .ToListAsync();

            var locations = dbProducts.Select(x => x.Location.Name).Distinct().ToList();

            var output = new List<ConsolidationDto>();

            foreach (var location in locations)
            {
                var serials = dbProducts
                    .Where(x => x.Location.Name == location)
                    .Select((p) => new ConsolidateSerialDto
                    {
                        SerialNo = p.SerialNumber
                    })
                    .ToList();
                
                output.Add(new()
                {
                    LocationName = location,
                    Serials = serials,
                    PartNumberName = request.SearchParams.PartNumberName,
                });

            }

            //foreach (var location in locations)
            //{

            //    var serials = new List<ConsolidateSerialDto>();

            //    var selectedProducts = dbProducts.Where(x => x.Location.Name == location).ToList();

            //    foreach (var serial in selectedProducts)
            //    {
            //        serials.Add(new()
            //        {
            //            SerialNo = serial.SerialNumber,

            //        });
            //    }
            //    output.Add(new()
            //    {
            //        LocationName = location,
            //        Serials = serials,
            //        PartNumberName = selectedProducts.First().PartNumber.Name,
            //    });

            //}
            return Result<List<ConsolidationDto>>.Success(output);

        }
    }

}
