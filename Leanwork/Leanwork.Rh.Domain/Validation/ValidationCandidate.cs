using FluentValidation;
using Leanwork.Rh.Domain.Validation.Helper;

namespace Leanwork.Rh.Domain;

public class ValidationCandidate : AbstractValidator<Candidate>
{
    public ValidationCandidate()
    {
        RuleFor(x => x.CompanyId)
           .Must(companyId => companyId != 0).WithMessage(string.Format(Resource.MessageFieldNotLinked, nameof(Candidate.CompanyId), nameof(Candidate)));

        RuleFor(x => x.GenderId)
           .Must(genderId => genderId != 0).WithMessage(string.Format(Resource.MessageFieldNotLinked, nameof(Candidate.GenderId), nameof(Candidate)));

        RuleFor(x => x.FirstName)
              .NotEmpty().WithMessage(string.Format(Resource.MessageBlankField, nameof(Candidate.FirstName)))
              .NotNull().WithMessage(string.Format(Resource.NullFieldMessage, nameof(Candidate.FirstName)))
              .Length(5, 100).WithMessage(string.Format(Resource.MessageMinimumFieldMaximumCharacters, nameof(Candidate.FirstName), 5, 100));

        RuleFor(x => x.LastName)
              .NotEmpty().WithMessage(string.Format(Resource.MessageBlankField, nameof(Candidate.LastName)))
              .NotNull().WithMessage(string.Format(Resource.NullFieldMessage, nameof(Candidate.LastName)))
              .Length(5, 100).WithMessage(string.Format(Resource.MessageMinimumFieldMaximumCharacters, nameof(Candidate.LastName), 5, 100));

        RuleFor(x => x.CPF)
             .NotEmpty().WithMessage(string.Format(Resource.MessageBlankField, nameof(Candidate.CPF)))
             .NotNull().WithMessage(string.Format(Resource.NullFieldMessage, nameof(Candidate.CPF)))
             .Must(CPFHelper.ValidateCPF).WithMessage(string.Format(Resource.MessageFieldCPFOrCNPJInvalid, nameof(Candidate.CPF)));

        RuleFor(x => x.DateOfBirth)
             .Must(DateOfBirth18.BeAtLeast18YearsOld).WithMessage(string.Format(Resource.MessageFieldDateOfBirth, nameof(Candidate.DateOfBirth), nameof(Candidate)));

    }
}
