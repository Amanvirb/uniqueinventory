using Application.Core;
using Microsoft.EntityFrameworkCore.Sqlite.Storage.Json.Internal;

namespace Application.Products;

public class DeleteOrderDetail
{
    public class Command : IRequest<Result<Unit>>
    {
        public int OrderId { get; set; }
        public string ProductName { get; set; }
    }
    public class Handler(DataContext context) : IRequestHandler<Command, Result<Unit>>
    {
        private DataContext _context = context;

        public async Task<Result<Unit>> Handle(Command request, CancellationToken ct)
        {
            //var dbOrder = await _context.Orders
            //    .Include(o => o.AppUser)
            //    .Include(x => x.OrderDetails).ThenInclude(p => p.ProductNumber)
            //     .FirstOrDefaultAsync(x => x.Id == request.OrderId, ct);

            //if (dbOrder is null) return Result<Unit>.Failure("Order not found");

            //var orderDetail = dbOrder.OrderDetails
            //    .FirstOrDefault(x => x.ProductNumber.Name == request.ProductName.Trim().ToUpper());

            var result = await _context.OrderDetails
                .Where(x => x.OrderId == request.OrderId
                && x.ProductNumber.Name == request.ProductName.Trim().ToUpper())
                .ExecuteDeleteAsync() > 0;

            //dbOrder.OrderDetails.Remove(orderDetail);

            //var result = await _context.SaveChangesAsync(ct) > 0;

            if (!result) return Result<Unit>.Failure("Failed to Delete Product");
            return Result<Unit>.Success(Unit.Value);
        }
    }
}
