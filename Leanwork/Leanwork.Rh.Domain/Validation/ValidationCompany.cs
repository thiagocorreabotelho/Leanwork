using FluentValidation;

namespace Leanwork.Rh.Domain;

public class ValidationCompany : AbstractValidator<Company>
{
    public ValidationCompany()
    {
        RuleFor(x => x.Name)
               .NotEmpty().WithMessage(string.Format(Resource.MessageBlankField, nameof(Company.Name)))
               .Length(1, 50).WithMessage(string.Format(Resource.MessageMinimumFieldMaximumCharacters, nameof(Company.Name), 1, 50));

        RuleFor(x => x.CNPJ)
               .NotEmpty().WithMessage(string.Format(Resource.MessageBlankField, nameof(Company.CNPJ)))
               .NotNull().WithMessage(string.Format(Resource.NullFieldMessage, nameof(Company.CNPJ)))
               .Must(CNPJValidation.ValidateCNPJ).WithMessage(string.Format(Resource.MessageFieldCPFOrCNPJInvalid, nameof(Company.CNPJ)));   
    }
}
