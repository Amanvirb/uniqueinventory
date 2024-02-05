using FluentValidation;

namespace Application.Products;

public class ProductValidator : AbstractValidator<ProductDto>
{
    public ProductValidator()
    {
        RuleFor(x => x.SerialNumber).NotEmpty();
        RuleFor(x => x.ProductName).NotEmpty();
        RuleFor(x => x.LocationName).NotEmpty();
    }
}
