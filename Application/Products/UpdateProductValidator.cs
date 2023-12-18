using FluentValidation;

namespace Application.Products;

public class UpdateProductValidator : AbstractValidator<UpdateProductDto>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.ProductName).NotEmpty();
        RuleFor(x => x.LocationName).NotEmpty();
    }
}
