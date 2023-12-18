using Application.Locations;
using FluentValidation;

namespace Application.Products;

public class EditProductName
{
    public class Command : IRequest<Result<Unit>>
    {
        public CommonDto Product { get; set; }
    }
    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Product).SetValidator(new CommonValidator());
        }
    }
    public class Handler(DataContext context) : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context = context;

        public async Task<Result<Unit>> Handle(Command request, CancellationToken ct)
        {
            bool result;

            var updatedProduct = request.Product;

            updatedProduct.Name = updatedProduct.Name.Trim().ToUpper();

            var dbProduct = await _context.ProductNumbers
                .Include(x => x.Products)
                .FirstOrDefaultAsync(x => x.Id == request.Product.Id, ct);

            if (dbProduct is null) return null;

            if (dbProduct.Name == updatedProduct.Name)
                return Result<Unit>.Failure("Entered product name is same as previous");

            result = await _context.ProductNumbers
                .Where(x=>x.Id== request.Product.Id)
                .ExecuteUpdateAsync(x => x.SetProperty(p => p.Name, updatedProduct.Name), ct) > 0;

            if (!result) return Result<Unit>.Failure("Failed to update Product name");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
