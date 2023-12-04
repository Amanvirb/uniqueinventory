namespace Application.PartNumbers;
public class AddPartNumber
{
    public class Command : IRequest<Result<Unit>>
    {
        public PartNumber PartNumber { get; set; }
    }
    public class Handler(DataContext context) : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context = context;

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var existingPartNumber = await _context.PartNumbers.FirstOrDefaultAsync(x => x.Name == request.PartNumber.Name, cancellationToken: cancellationToken);
           
            if (existingPartNumber is not null) return Result<Unit>.Failure("Part Number already exists");

            var partNumber = new PartNumber
            {
                Name = request.PartNumber.Name.Trim().ToUpper(),
            };
        
            _context.PartNumbers.Add(partNumber);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (!result) return Result<Unit>.Failure("Cannot Add Part Number");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
