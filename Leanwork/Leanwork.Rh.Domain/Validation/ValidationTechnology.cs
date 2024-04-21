using FluentValidation;
using Leanwork.Rh.Domain.Entitie;

namespace Leanwork.Rh.Domain.Validation;

public class ValidationTechnology : AbstractValidator<Technology>
{
    public ValidationTechnology()
    {
        RuleFor(x => x.Name)
               .NotEmpty().WithMessage(string.Format(Resource.MessageBlankField, nameof(Technology.Name)))
               .Length(1, 30).WithMessage(string.Format(Resource.MessageMinimumFieldMaximumCharacters, nameof(Technology.Name), 1, 30));
    }
}
