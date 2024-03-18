using Application.Locations;
using Application.ProductNames.Dtoæ;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.Xml.Linq;

namespace Application.ProductNames;
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
            string newName = request.Product.Name.Trim().ToUpper();

            ProductName newProductName = new()
            {
                Name = newName,
                Slug = request.Product.Slug,
                Description = request.Product.Description,
                Price = request.Product.Price,
                Tags = request.Product.Tags,
            };


            var existingProductName = await _context.ProductNames.FirstOrDefaultAsync(x => x.Name == newName, ct);

            if (existingProductName is not null) return Result<Unit>.Failure("Product Name already exists");


            _context.ProductNames.Add(newProductName);

            var result = await _context.SaveChangesAsync(ct) > 0;
            if (!result) return Result<Unit>.Failure("Cannot Add Product Name");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
