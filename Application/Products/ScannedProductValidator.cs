using FluentValidation;

namespace Application.Products;

public class ScannedProductValidator : AbstractValidator<ScannedQRDto>
{
    public ScannedProductValidator()
    {
        RuleFor(x => x.LocationName).NotEmpty();
        RuleFor(x => x.Quantity).NotEmpty();
        RuleForEach(x => x.Products).ChildRules(p =>
        {
            p.RuleFor(x => x.SerialNo).NotEmpty();
            p.RuleFor(x => x.ProductName).NotEmpty();
        });
    }
}
