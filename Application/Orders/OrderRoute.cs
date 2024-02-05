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
                .Include(o=>o.OrderDetails).ThenInclude(x => x.ProductName)
                .FirstOrDefaultAsync(x =>x.OrderId == request.OrderId, ct);

            if (dbOrder is null) return Result<List<OrderRouteDto>>.Failure("Order does not exist");

            foreach (var product in dbOrder.OrderDetails)
            {
                var products = await _context.Products
                      .Include(x => x.Location)
                      .Include(x => x.ProductName)
                      .Where(x => x.ProductName == product.ProductName)
                      .ToListAsync(ct);

                orderedProducts.AddRange(products);
            }

            var locations = orderedProducts.Select(x => x.Location.Name).Distinct().ToList();

            foreach (var location in locations)
            {
                var products = orderedProducts
                     .Where(x => x.Location.Name == location)
                     .ToList();

                var productNames = products.Select(x => x.ProductName.Name).Distinct().ToList();

                var reqProd = new List<RequestedProductDto>();

                foreach (var productName in productNames)
                {
                    reqProd.Add(new()
                    {
                        ProductName = productName,
                        AvailableProductNameQuantity = products.Where(x => x.ProductName.Name == productName).Count(),
                        ReqQty = dbOrder.OrderDetails.FirstOrDefault(x => x.ProductName.Name == productName).Quantity
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
