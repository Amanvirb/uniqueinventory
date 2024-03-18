using Application.Locations;
using Application.ProductNames.Dtoæ;
using FluentValidation;

namespace Application.ProductNames;

public class EditProductName
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
            bool result;

            var dbProduct = await _context.ProductNames
               .Include(x => x.Products)
               .FirstOrDefaultAsync(x => x.Id == request.Product.Id, ct);

            if (dbProduct is null) return Result<Unit>.Failure("Product name does not exist, please enter valid product");

            //if (dbProduct.Name == request.Product.Name)
            //    return Result<Unit>.Failure("Entered product name is same as previous");

            if (dbProduct.Name != request.Product.Name ||
                dbProduct.Description != request.Product.Description ||
                dbProduct.Price != request.Product.Price ||
                dbProduct.Tags != request.Product.Tags
                )
            {
                dbProduct.Name = request.Product.Name;
                dbProduct.Description = request.Product.Description;
                dbProduct.Price = request.Product.Price;
                dbProduct.Tags = request.Product.Tags;
            }
            else
            {
                return Result<Unit>.Failure("Entered product details are same as previous");
            }

            _context.Entry(dbProduct).State = EntityState.Modified;
            result = await _context.SaveChangesAsync(ct) > 0;

            if (!result) return Result<Unit>.Failure("Failed to update Product name");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
