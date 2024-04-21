using FluentValidation;
using Leanwork.Rh.Domain.Entitie;

namespace Leanwork.Rh.Domain.Validation;

public class ValidationJobInterviewWeight : AbstractValidator<JobInterviewWeight>
{
    public ValidationJobInterviewWeight()
    {
        RuleFor(x => x.TechnologyId)
           .Must(technologyId => technologyId != 0).WithMessage(string.Format(Resource.MessageFieldNotLinked, nameof(JobInterviewWeight.TechnologyId), nameof(JobInterviewWeight)));

        RuleFor(x => x.JobOpeningId)
           .Must(jobOpeningId => jobOpeningId != 0).WithMessage(string.Format(Resource.MessageFieldNotLinked, nameof(JobInterviewWeight.JobOpeningId), nameof(JobInterviewWeight)));
    }
}
