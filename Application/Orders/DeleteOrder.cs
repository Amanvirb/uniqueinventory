namespace Application.Products;

public class DeleteOrder
{
    public class Command : IRequest<Result<Unit>>
    {
        public string OrderId { get; set; }
    }
    public class Handler(DataContext context) : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context = context;

        public async Task<Result<Unit>> Handle(Command request, CancellationToken ct)
        {
            var result = await _context.Orders
                 .Where(x => x.OrderId == request.OrderId)
                 .ExecuteDeleteAsync(ct) > 0;

            if (!result) return Result<Unit>.Failure("Failed to Delete Order");
            return Result<Unit>.Success(Unit.Value);
        }
    }
}
