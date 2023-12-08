using Domain;

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

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            bool result;

            var dbOrder = await _context.Orders
                .Include(a => a.AppUser)
                .Include(o => o.OrderDetails).ThenInclude(p => p.ProductNumber)
                .FirstOrDefaultAsync(x => x.Id == request.Order.Id
                && x.Confirmed == false, cancellationToken: cancellationToken);

            if (dbOrder is null) return null;

            var orderDetail = dbOrder.OrderDetails
                .FirstOrDefault(x => x.ProductNumber.Name == request.Order.ProductName.Trim().ToUpper());

            if (orderDetail is null)
            {
                var newOrderDetails = new OrderDetail
                {
                    Quantity = request.Order.Quantity,
                    ProductNumber = new ProductNumber { Name = request.Order.ProductName.Trim().ToUpper() },
                    Order = dbOrder,
                };

                _context.OrderDetails.Add(newOrderDetails);
                result = await _context.SaveChangesAsync(cancellationToken) > 0;

            }
            else
            {
                orderDetail.Quantity = request.Order.Quantity;

                _context.Entry(dbOrder).State = EntityState.Modified;
                result = await _context.SaveChangesAsync(cancellationToken) > 0;
            }

            if (!result) Result<Unit>.Failure("Can not create order");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
