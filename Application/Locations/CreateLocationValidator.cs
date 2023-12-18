using FluentValidation;

namespace Application.Locations;

public class CreateLocationValidator : AbstractValidator<AddLocationDto>
{
    public CreateLocationValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.TotalCapacity).NotNull();
    }
}
