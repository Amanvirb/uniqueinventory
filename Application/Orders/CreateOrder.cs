using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Application.Orders;
public class CreateOrder
{
    public class Command : IRequest<Result<Unit>>
    {
        public CreateOrderDto Order { get; set; }
    }
    public class Handler(DataContext context) : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context = context;

        public async Task<Result<Unit>> Handle(Command request, CancellationToken ct)
        {
            bool result;

            var productNumberName = await _context.ProductNumbers
                .Include(x => x.Products)
                .FirstOrDefaultAsync(p => p.Name == request.Order.ProductName.Trim().ToUpper(), ct);

            if (productNumberName is null) return null;

            var orderDetail = new OrderDetail
            {
                Quantity = request.Order.Quantity,
                ProductNumber = productNumberName,
            };

            var order = new Order
            {
                Confirmed = false,
                Packed = false,
                OrderDetails = new[] { orderDetail }
            };

            _context.Orders.Add(order);

            result = await _context.SaveChangesAsync(ct) > 0;
            if (!result) Result<Unit>.Failure("Can not create order");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
