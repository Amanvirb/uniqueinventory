namespace Application.Orders;
public class UpdateOrder
{
    public class Command : IRequest<Result<Unit>>
    {
        public UpdateOrderDto Order { get; set; }
    }
    public class Handler(DataContext context) : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context = context;

        public async Task<Result<Unit>> Handle(Command request, CancellationToken ct)
        {
            var product = request.Order.ProductName;

            if (request.Order.Quantity < 1)
            {
                await _context.OrderDetails
                    .Where(x => x.OrderId == request.Order.OrderId && x.ProductName.Name == product)
                    .ExecuteDeleteAsync(ct);

                return Result<Unit>.Success(Unit.Value);
            }

            bool result;

            var dbOrder = await _context.Orders
                .Include(o => o.OrderDetails).ThenInclude(p => p.ProductName)
                .FirstOrDefaultAsync(x => x.OrderId == request.Order.OrderId
                && !x.Confirmed, ct);

            if (dbOrder is null) return Result<Unit>.Failure("Order does not exist");

            var orderDetail = dbOrder.OrderDetails
                .FirstOrDefault(x => x.ProductName.Name == product);

            if (orderDetail is null)
            {
                var newOrderDetails = new OrderDetail
                {
                    Quantity = request.Order.Quantity,
                    ProductName = new ProductName { Name = product },
                    Order = dbOrder,
                };

                _context.OrderDetails.Add(newOrderDetails);
                result = await _context.SaveChangesAsync(ct) > 0;
            }
            else
            {
                result = await _context.OrderDetails
                      .Where(x => x.Id == orderDetail.Id)
                      .ExecuteUpdateAsync(x => x.SetProperty(x => x.Quantity, request.Order.Quantity), ct) > 0;
            }

            if (!result) Result<Unit>.Failure("Can not update order");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}