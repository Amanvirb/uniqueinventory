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
            var reqProd = new List<RequestedProductDto>();

            foreach (var partNumber in request.Order)
            {
                var products = await _context.Products
                  .Include(x => x.Location)
                  .Include(x => x.PartNumber)
                  .Where(x => x.PartNumber.Name == partNumber.PartNumber)
                  .ToListAsync();

                dbProducts.AddRange(products);

            }

            var locations = dbProducts.Select(x => x.Location.Name).Distinct().ToList();

            var reqQty = new List<RequestedProductDto>();


            foreach (var location in locations)
            {

                var partNumbers = dbProducts
                     .Where(x => x.Location.Name == location)
                     .ToList();

                var partNumberNames = partNumbers.Select(x => x.PartNumber.Name).Distinct().ToList();

                foreach (var partNumberName in partNumberNames)
                {

                    output.Add(new()
                    {
                        LocationName = location,
                        PartNumber = partNumberName,
                        AvailableQty = partNumbers.Where(x => x.PartNumber.Name == partNumberName).Count(),
                    });


                    //reqQty.Add(new()
                    //{
                    //    PartNumberName = partNumberName,
                    //    AvailablePartNumberQuantity = partNumbers.Where(x => x.PartNumber.Name == partNumberName).Count(),
                    //});
                }

                //output.Add(new()
                //{
                //    LocationName = location,
                //    ReqProducts = reqQty
                //});
            }

            //var locations = dbProducts.Select(x => x.Location).Distinct().ToList();

            //foreach (var location in locations)
            //{



            //    foreach(var partNumber in location.Products)
            //    {

            //    }



            //   // var partNumbers = dbProducts
            //   //.Where(x => x.Location == location)
            //   //.Select((p) => new OrderRouteDto
            //   //{
            //   //    PartNumber = p.PartNumber.Name,
            //   //    LocationName = location.Name,
            //   //    AvailableQty = location.Products.Count()
            //   //})
            //   //.ToList();

            //   // output.AddRange(partNumbers);

            //}



            return Result<List<OrderRouteDto>>.Success(output);

        }
    }
}
