using Domain;
using System.Security.Cryptography.X509Certificates;

namespace Application.Orders;
public class OrderRoute
{
    public class Query : IRequest<Result<List<OrderRouteDto>>>
    {
        public OrderRoutePayloadDto[] Order { get; set; }
    }
    public class Handler(DataContext context, IMapper mapper) : IRequestHandler<Query, Result<List<OrderRouteDto>>>
    {
        private readonly DataContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<List<OrderRouteDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            List<Product> dbProducts = new();

            var output = new List<OrderRouteDto>();


            foreach (var partNumber in request.Order)
            {
                var products = await _context.Products
                  .Include(x => x.Location)
                  .Include(x => x.PartNumber)
                  .Where(x => x.PartNumber.Name == partNumber.PartNumber.Trim().ToUpper())
                  .ToListAsync();

                dbProducts.AddRange(products);
            }

            var locations = dbProducts.Select(x => x.Location.Name).Distinct().ToList();

            foreach (var location in locations)
            {
                var products = dbProducts
                     .Where(x => x.Location.Name == location)
                     .ToList();

                var partNumberNames = products.Select(x => x.PartNumber.Name).Distinct().ToList();

                var reqProd = new List<RequestedProductDto>();

                foreach (var partNumberName in partNumberNames)
                {

                    //output.Add(new()
                    //{
                    //    LocationName = location,
                    //    PartNumber = partNumberName,
                    //    AvailableQty = partNumbers.Where(x => x.PartNumber.Name == partNumberName).Count(),
                    //    ReqQty = request.Order.Where(x => x.PartNumber == partNumberName).Select(x => x.ReqQty).Sum()
                    //});


                    reqProd.Add(new()
                    {
                        PartNumberName = partNumberName,
                        AvailablePartNumberQuantity = products.Where(x => x.PartNumber.Name == partNumberName).Count(),
                        ReqQty = request.Order.Where(x => x.PartNumber == partNumberName).Select(x => x.ReqQty).Sum()
                    });
                }

                output.Add(new()
                {
                    LocationName = location,
                    ReqProducts = reqProd
                });
            }
            return Result<List<OrderRouteDto>>.Success(output);

        }
    }
}
