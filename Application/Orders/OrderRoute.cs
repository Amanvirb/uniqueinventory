using Domain;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Application.Orders;
public class OrderRoute
{
    public class Query : IRequest<Result<OrderRouteDto>>
    {
        public int Id { get; set; }
    }
    public class Handler(DataContext context, IMapper mapper) : IRequestHandler<Query, Result<OrderRouteDto>>
    {
        private readonly DataContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<OrderRouteDto>> Handle(Query request, CancellationToken cancellationToken)
        {

            var output = new OrderRouteDto();
           
            List<Product> orderedProducts = new();

            var dbOrder = await _context.Orders
                //.Include(a=>a.AppUser)
                .Include(o=>o.OrderDetails)
                .FirstOrDefaultAsync(x=>x.Id == request.Id);
            if (dbOrder is null) return null;
            
            foreach (var product in dbOrder.OrderDetails)
            {
                var products = await _context.Products
                      .Include(x => x.Location)
                      .Include(x => x.PartNumber)
                      .Where(x => x.PartNumber == product.ProductNumber)
                      .ToListAsync();

                orderedProducts.AddRange(products);
            }

            var locations = orderedProducts.Select(x => x.Location.Name).Distinct().ToList();

            foreach (var location in locations)
            {
                var products = orderedProducts
                     .Where(x => x.Location.Name == location)
                     .ToList();

                var partNumberNames = products.Select(x => x.PartNumber.Name).Distinct().ToList();

                var reqProd = new List<RequestedProductDto>();

                foreach (var partNumberName in partNumberNames)
                {
                    reqProd.Add(new()
                    {
                        PartNumberName = partNumberName,
                        AvailablePartNumberQuantity = products.Where(x => x.PartNumber.Name == partNumberName).Count(),
                        ReqQty = dbOrder.OrderDetails.Where(x => x.ProductNumber.Name == partNumberName).Select(x => x.Quantity).Sum()
                    });
                }

                output = new OrderRouteDto
                {
                    LocationName = location,
                    ReqProducts = reqProd
                };

               
            }
            return Result<OrderRouteDto>.Success(output);
        }
    }
}
