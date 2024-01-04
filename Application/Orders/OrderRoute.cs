namespace Application.Orders;
public class OrderRoute
{
    public class Query : IRequest<Result<List<OrderRouteDto>>>
    {
        public string OrderId { get; set; }
    }
    public class Handler(DataContext context) : IRequestHandler<Query, Result<List<OrderRouteDto>>>
    {
        private readonly DataContext _context = context;

        public async Task<Result<List<OrderRouteDto>>> Handle(Query request, CancellationToken ct)
        {

            var output = new List<OrderRouteDto>();

            List<Product> orderedProducts = [];

            var dbOrder = await _context.Orders
                .Include(a => a.AppUser)
                .Include(o=>o.OrderDetails).ThenInclude(x => x.ProductNumber)
                .FirstOrDefaultAsync(x =>x.OrderId == request.OrderId, ct);

            if (dbOrder is null) return Result<List<OrderRouteDto>>.Failure("Order does not exist");

            foreach (var product in dbOrder.OrderDetails)
            {
                var products = await _context.Products
                      .Include(x => x.Location)
                      .Include(x => x.ProductNumber)
                      .Where(x => x.ProductNumber == product.ProductNumber)
                      .ToListAsync(ct);

                orderedProducts.AddRange(products);
            }

            var locations = orderedProducts.Select(x => x.Location.Name).Distinct().ToList();

            foreach (var location in locations)
            {
                var products = orderedProducts
                     .Where(x => x.Location.Name == location)
                     .ToList();

                var productNames = products.Select(x => x.ProductNumber.Name).Distinct().ToList();

                var reqProd = new List<RequestedProductDto>();

                foreach (var productName in productNames)
                {
                    reqProd.Add(new()
                    {
                        ProductNumberName = productName,
                        AvailableProductNumberQuantity = products.Where(x => x.ProductNumber.Name == productName).Count(),
                        ReqQty = dbOrder.OrderDetails.FirstOrDefault(x => x.ProductNumber.Name == productName).Quantity
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
