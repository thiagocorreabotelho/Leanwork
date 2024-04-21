using FluentValidation;

namespace Leanwork.Rh.Domain;

public class ValidationCandidateTechnology : AbstractValidator<CandidateTechnologyRel>
{
    public ValidationCandidateTechnology()
    {
        RuleFor(x => x.CandidateId)
            .Must(candidateId => candidateId != 0).WithMessage(string.Format(Resource.MessageFieldNotLinked, nameof(CandidateTechnologyRel.CandidateId), nameof(CandidateTechnologyRel)));

         RuleFor(x => x.TechnologyId)
            .Must(technologyId => technologyId != 0).WithMessage(string.Format(Resource.MessageFieldNotLinked, nameof(CandidateTechnologyRel.TechnologyId), nameof(CandidateTechnologyRel)));
    }
}
