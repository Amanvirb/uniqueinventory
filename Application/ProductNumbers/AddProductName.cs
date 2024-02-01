using Application.Locations;
using Application.ProductNumbers.Dtoæ;
using FluentValidation;

namespace Application.ProductNumbers;
public class AddProductName
{
    public class Command : IRequest<Result<Unit>>
    {
        public AddProductNameDto Product { get; set; }
    }
    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Product).SetValidator(new AddProductNameValidator());
        }
    }
    public class Handler(DataContext context) : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context = context;

        public async Task<Result<Unit>> Handle(Command request, CancellationToken ct)
        {
            var name = request.Product.Name.Trim().ToUpper();

            var existingProductNumber = await _context.ProductNumbers.FirstOrDefaultAsync(x => x.Name == name, ct);

            if (existingProductNumber is not null) return Result<Unit>.Failure("Product Name already exists");

            _context.ProductNumbers.Add(new ProductNumber { Name = name});

            var result = await _context.SaveChangesAsync(ct) > 0;
            if (!result) return Result<Unit>.Failure("Cannot Add Part Number");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
