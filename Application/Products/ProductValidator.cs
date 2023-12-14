using FluentValidation;

namespace Application.Products;

public class ProductValidator : AbstractValidator<ProductDto>
{
    public ProductValidator()
    {
        RuleFor(x => x.SerialNumber).NotEmpty();
        RuleFor(x => x.ProductNumberName).NotEmpty();
        RuleFor(x => x.LocationName).NotEmpty();
    }
}
