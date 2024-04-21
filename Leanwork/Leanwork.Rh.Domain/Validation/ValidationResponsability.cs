using FluentValidation;

namespace Leanwork.Rh.Domain;

public class ValidationResponsability : AbstractValidator<Responsibility>
{
    public ValidationResponsability()
    {
        RuleFor(x => x.Description)
               .NotEmpty().WithMessage(string.Format(Resource.MessageBlankField, nameof(Responsibility.Description)))
               .NotNull().WithMessage(string.Format(Resource.NullFieldMessage, nameof(Responsibility.Description)))
               .Length(1, 150).WithMessage(string.Format(Resource.MessageMinimumFieldMaximumCharacters, nameof(Responsibility.Description), 1, 150));
    }
}
