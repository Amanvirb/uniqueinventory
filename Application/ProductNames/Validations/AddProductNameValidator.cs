using Application.ProductNames.Dtoæ;
using FluentValidation;

namespace Application.Locations;

public class AddProductNameValidator : AbstractValidator<AddProductNameDto>
{
    public AddProductNameValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
