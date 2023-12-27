namespace Application.Products;
public class DeleteOrderDetail
{
    public class Command : IRequest<Result<Unit>>
    {
        public string OrderId { get; set; }
        public string ProductName { get; set; }
    }
    public class Handler(DataContext context) : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context = context;

        public async Task<Result<Unit>> Handle(Command request, CancellationToken ct)
        {
            var result = await _context.OrderDetails
                .Where(x => x.OrderId == request.OrderId
                && x.ProductNumber.Name == request.ProductName)
                .ExecuteDeleteAsync(ct) > 0;

            if (!result) return Result<Unit>.Failure("Failed to Delete Product");
            return Result<Unit>.Success(Unit.Value);
        }
    }
}
