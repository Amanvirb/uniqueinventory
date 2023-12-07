namespace Application.Products;

public class DeleteOrder
{
    public class Command : IRequest<Result<Unit>>
    {
        public int Id { get; set; }
    }
    public class Handler(DataContext context) : IRequestHandler<Command, Result<Unit>>
    {
        private DataContext _context = context;

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var dbOrder = await _context.Orders
                .Include(o => o.AppUser)
                .Include(x => x.OrderDetails)
                 .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

            if (dbOrder is null) return Result<Unit>.Failure("Order not found");
          
            _context.Orders.Remove(dbOrder);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!result) return Result<Unit>.Failure("Failed to Delete Product");
            return Result<Unit>.Success(Unit.Value);
        }
    }
}
