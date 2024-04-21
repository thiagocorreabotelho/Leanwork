using FluentValidation;
using Leanwork.Rh.Domain.Validation.Helper;

namespace Leanwork.Rh.Domain;

public class ValidationAddress : AbstractValidator<Address>
{
    public ValidationAddress()
    {
        RuleFor(x => x.Name)
               .NotEmpty().WithMessage(string.Format(Resource.MessageBlankField, nameof(Address.Name)))
               .NotNull().WithMessage(string.Format(Resource.NullFieldMessage, nameof(Address.Name)))
               .Length(5, 50).WithMessage(string.Format(Resource.MessageMinimumFieldMaximumCharacters, nameof(Address.Name), 5, 50));

        RuleFor(x => x.ZipCode)
               .NotEmpty().WithMessage(string.Format(Resource.MessageBlankField, nameof(Address.ZipCode)))
               .NotNull().WithMessage(string.Format(Resource.NullFieldMessage, nameof(Address.ZipCode)))
               .Length(9).WithMessage(string.Format(Resource.MessageMaximumFieldCharacters, nameof(Address.ZipCode), 9));

        RuleFor(x => x.Street)
               .NotEmpty().WithMessage(string.Format(Resource.MessageBlankField, nameof(Address.Street)))
               .NotNull().WithMessage(string.Format(Resource.NullFieldMessage, nameof(Address.Street)))
               .Length(5, 100).WithMessage(string.Format(Resource.MessageMinimumFieldMaximumCharacters, nameof(Address.Street), 5, 100));

        RuleFor(x => x.Number)
               .NotEmpty().WithMessage(string.Format(Resource.MessageBlankField, nameof(Address.Number)))
               .NotNull().WithMessage(string.Format(Resource.NullFieldMessage, nameof(Address.Number)))
               .Length(1, 5).WithMessage(string.Format(Resource.MessageMinimumFieldMaximumCharacters, nameof(Address.Number), 5, 100));

        RuleFor(x => x.Neighborhood)
               .NotEmpty().WithMessage(string.Format(Resource.MessageBlankField, nameof(Address.Neighborhood)))
               .NotNull().WithMessage(string.Format(Resource.NullFieldMessage, nameof(Address.Neighborhood)))
               .Length(1, 100).WithMessage(string.Format(Resource.MessageMinimumFieldMaximumCharacters, nameof(Address.Neighborhood), 1, 100));

         RuleFor(x => x.City)
               .NotEmpty().WithMessage(string.Format(Resource.MessageBlankField, nameof(Address.City)))
               .NotNull().WithMessage(string.Format(Resource.NullFieldMessage, nameof(Address.City)))
               .Length(1, 100).WithMessage(string.Format(Resource.MessageMinimumFieldMaximumCharacters, nameof(Address.City), 1, 100));

        RuleFor(x => x.State)
               .NotEmpty().WithMessage(string.Format(Resource.MessageBlankField, nameof(Address.State)))
               .NotNull().WithMessage(string.Format(Resource.NullFieldMessage, nameof(Address.State)))
               .Length(2).WithMessage(string.Format(Resource.MessageMaximumFieldCharacters, nameof(Address.State), 2))
               .Must(StateHelper.ValidationState).WithMessage(string.Format(Resource.MessageFieldStateInvalid, nameof(Address.State)));
    }
}
