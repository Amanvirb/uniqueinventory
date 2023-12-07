namespace Application.ProductNumbers;
public class AddProductNumber
{
    public class Command : IRequest<Result<Unit>>
    {
        public ProductNumber ProductNumber { get; set; }
    }
    public class Handler(DataContext context) : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context = context;

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var newProductNumber = request.ProductNumber.Name.Trim().ToUpper();

            var existingProductNumber = await _context.ProductNumbers.FirstOrDefaultAsync(x => x.Name == newProductNumber,
                cancellationToken: cancellationToken);

            if (existingProductNumber is not null) return Result<Unit>.Failure("Part Number already exists");

            var partNumber = new ProductNumber
            {
                Name = newProductNumber,
            };

            _context.ProductNumbers.Add(partNumber);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (!result) return Result<Unit>.Failure("Cannot Add Part Number");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
