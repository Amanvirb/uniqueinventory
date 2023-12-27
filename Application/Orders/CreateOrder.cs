using Application.Interfaces;

namespace Application.Orders;
public class CreateOrder
{
    public class Command : IRequest<Result<Unit>>
    {
        public CreateOrderDto Order { get; set; }
    }
    public class Handler(DataContext context, IUserAccessor userAccessor) : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context = context;
        private readonly IUserAccessor _userAccessor = userAccessor;

        public async Task<Result<Unit>> Handle(Command request, CancellationToken ct)
        {
            bool result;            

            var productName = await _context.ProductNumbers
                .Include(x => x.Products)
                .FirstOrDefaultAsync(p => p.Name == request.Order.ProductName.Trim().ToUpper(), ct);

            if (productName is null) return null;

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername(), ct);

            var orderDetail = new OrderDetail
            {
                Quantity = request.Order.Quantity,
                ProductNumber = productName,
            };

            var order = new Order
            {
                OrderId = Guid.NewGuid().ToString(),
                Confirmed = false,
                Packed = false,
                OrderDetails = new[] { orderDetail },
                AppUser = user
            };

            _context.Orders.Add(order);

            result = await _context.SaveChangesAsync(ct) > 0;
            if (!result) Result<Unit>.Failure("Can not create order");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
