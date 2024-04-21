using FluentValidation;

namespace Leanwork.Rh.Domain;

public class ValidationJobOpening : AbstractValidator<JobOpening>
{
    public ValidationJobOpening()
    {
        RuleFor(x => x.Title)
               .NotEmpty().WithMessage(string.Format(Resource.MessageBlankField, nameof(JobOpening.Title)))
               .Length(1, 30).WithMessage(string.Format(Resource.MessageMinimumFieldMaximumCharacters, nameof(JobOpening.Title), 1, 30));

         RuleFor(x => x.Summary)
               .NotEmpty().WithMessage(string.Format(Resource.MessageBlankField, nameof(JobOpening.Description)))
               .Length(10, 100).WithMessage(string.Format(Resource.MessageMinimumFieldMaximumCharacters, nameof(JobOpening.Description), 10, 100));

    }
}
