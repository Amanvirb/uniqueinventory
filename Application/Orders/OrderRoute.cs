using Domain;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Application.Orders;
public class OrderRoute
{
    public class Query : IRequest<Result<List<OrderRouteDto>>>
    {
        public int Id { get; set; }
    }
    public class Handler(DataContext context, IMapper mapper) : IRequestHandler<Query, Result<List<OrderRouteDto>>>
    {
        private readonly DataContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<List<OrderRouteDto>>> Handle(Query request, CancellationToken cancellationToken)
        {

            var output = new List<OrderRouteDto>();

            List<Product> orderedProducts = new();

            var dbOrder = await _context.Orders
                //.Include(a=>a.AppUser)
                .Include(o=>o.OrderDetails).ThenInclude(x => x.ProductNumber)
                .FirstOrDefaultAsync(x=>x.Id == request.Id);
            if (dbOrder is null) return null;
            
            foreach (var product in dbOrder.OrderDetails)
            {
                var products = await _context.Products
                      .Include(x => x.Location)
                      .Include(x => x.ProductNumber)
                      .Where(x => x.ProductNumber == product.ProductNumber)
                      .ToListAsync();

                orderedProducts.AddRange(products);
            }

            var locations = orderedProducts.Select(x => x.Location.Name).Distinct().ToList();

            foreach (var location in locations)
            {
                var products = orderedProducts
                     .Where(x => x.Location.Name == location)
                     .ToList();

                var partNumberNames = products.Select(x => x.ProductNumber.Name).Distinct().ToList();

                var reqProd = new List<RequestedProductDto>();

                foreach (var partNumberName in partNumberNames)
                {
                    reqProd.Add(new()
                    {
                        ProductNumberName = partNumberName,
                        AvailableProductNumberQuantity = products.Where(x => x.ProductNumber.Name == partNumberName).Count(),
                        ReqQty = dbOrder.OrderDetails.FirstOrDefault(x => x.ProductNumber.Name == partNumberName).Quantity
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
