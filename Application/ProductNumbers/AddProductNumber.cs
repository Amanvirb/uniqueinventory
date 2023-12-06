namespace Application.ProductNumbers;
public class AddProductNumber
{
    public class Command : IRequest<Result<Unit>>
    {
        public ProductNumber PartNumber { get; set; }
    }
    public class Handler(DataContext context) : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context = context;

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var newPartNumber = request.PartNumber.Name.Trim().ToUpper();

            var existingPartNumber = await _context.ProductNumbers.FirstOrDefaultAsync(x => x.Name == newPartNumber,
                cancellationToken: cancellationToken);

            if (existingPartNumber is not null) return Result<Unit>.Failure("Part Number already exists");

            var partNumber = new ProductNumber
            {
                Name = newPartNumber,
            };

            _context.ProductNumbers.Add(partNumber);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (!result) return Result<Unit>.Failure("Cannot Add Part Number");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
