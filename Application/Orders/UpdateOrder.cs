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
                //.FirstOrDefaultAsync(x => x.OrderNumber == request.Order.OrderNumber
                && x.Confirmed == false, cancellationToken: cancellationToken);

            var productNumberName = await _context.ProductNumbers
                .Include(x => x.Products)
                .FirstOrDefaultAsync(p => p.Name == request.Order.OrderDetails
                .Select(s => s.ProductNumber).First().Trim().ToUpper(), cancellationToken: cancellationToken);

            if (productNumberName is null) return null;

            if (dbOrder is not null)
            {
                //var orderId = dbOrder.Id;

                var updatedOrderDetail = new List<OrderDetailDto>();

                foreach (var dbOrderDetail in dbOrder.OrderDetails)
                {
                    dbOrderDetail.Quantity = request.Order.OrderDetails.Where(o => o.ProductNumber == dbOrderDetail.ProductNumber.Name).Select(o => o.Quantity).Sum();
                    
                    //updatedOrderDetail.Add(new()
                    //{
                    //    Quantity = request.Order.OrderDetails.Where(o => o.ProductNumber == productNumberName.Name).Select(o => o.Quantity).Sum(),

                    //    //Quantity = request.Order.OrderDetails.Where(o => o.ProductNumber == dbOrderDetail.ProductNumber.Name).Select(o => o.Quantity).Sum(),
                    //    ProductNumber = dbOrderDetail.ProductNumber.Name,
                    //});

                    //dbOrder.OrderDetails.Add(new()
                    //{
                    //    Quantity = request.Order.OrderDetails.Where(o => o.ProductNumber == productNumberName.Name).Select(o => o.Quantity).Sum(),

                    //});
                    //dbOrderDetail.Quantity = 12;

                    _context.Entry(dbOrder).State = EntityState.Modified;
                }
            }

            result = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (!result) Result<Unit>.Failure("Can not create order");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
