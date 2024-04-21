using FluentValidation;

using Leanwork.Rh.Domain.Entitie;

namespace Leanwork.Rh.Domain.Validation;

public class ValidationInterview : AbstractValidator<Interview>
{
    public ValidationInterview()
    {
        RuleFor(x => x.CandidateId)
            .Must(candidateId => candidateId != 0).WithMessage(string.Format(Resource.MessageFieldNotLinked, nameof(Interview.CandidateId), nameof(Interview)));

        RuleFor(x => x.JobOpeningId)
            .Must(jobOpeningId => jobOpeningId != 0).WithMessage(string.Format(Resource.MessageFieldNotLinked, nameof(Interview.JobOpeningId), nameof(Interview)));
    }
}
