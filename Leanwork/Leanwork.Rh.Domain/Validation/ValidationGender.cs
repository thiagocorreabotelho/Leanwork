using FluentValidation;

namespace Leanwork.Rh.Domain;

public class ValidationGender : AbstractValidator<Gender>
{
    public ValidationGender()
    {
         RuleFor(x => x.Name)
               .NotEmpty().WithMessage(string.Format(Resource.MessageBlankField, nameof(Gender.Name)))
               .Length(1, 20).WithMessage(string.Format(Resource.MessageMinimumFieldMaximumCharacters, nameof(Gender.Name), 1, 20));
    }
}
