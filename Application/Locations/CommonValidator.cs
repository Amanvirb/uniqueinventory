using FluentValidation;

namespace Application.Locations;

public class CommonValidator : AbstractValidator<CommonDto>
{
    public CommonValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
