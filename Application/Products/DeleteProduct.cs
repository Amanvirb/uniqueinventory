namespace Application.Products;
public class DeleteProduct
{
    public class Command : IRequest<Result<Unit>>
    {
        public int Id { get; set; }
    }
    public class Handler(DataContext context) : IRequestHandler<Command, Result<Unit>>
    {
        private DataContext _context = context;

        public async Task<Result<Unit>> Handle(Command request, CancellationToken ct)
        {
            //var dbProduct = await _context.Products
            //     .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

            //if (dbProduct is null) return Result<Unit>.Failure("Product not found");

            //_context.Products.Remove(dbProduct);

            //var result = await _context.SaveChangesAsync(ct) > 0;

            var result = await _context.Products
                .Where(x => x.Id == request.Id)
                .ExecuteDeleteAsync() > 0;

            if (!result) return Result<Unit>.Failure("Failed to Delete Product");
            return Result<Unit>.Success(Unit.Value);
        }
    }
}