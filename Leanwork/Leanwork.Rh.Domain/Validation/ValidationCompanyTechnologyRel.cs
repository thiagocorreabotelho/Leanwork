using FluentValidation;

namespace Leanwork.Rh.Domain;

public class ValidationCompanyTechnologyRel : AbstractValidator<CompanyTechnologyRel>
{
    public ValidationCompanyTechnologyRel()
    {
        RuleFor(x => x.CompanyId)
            .Must(companyId => companyId != 0).WithMessage(string.Format(Resource.MessageFieldNotLinked, nameof(CompanyTechnologyRel.CompanyId), nameof(CompanyTechnologyRel)));

        RuleFor(x => x.TechnologyId)
           .Must(technologyId => technologyId != 0).WithMessage(string.Format(Resource.MessageFieldNotLinked, nameof(CompanyTechnologyRel.TechnologyId), nameof(CompanyTechnologyRel)));
    }
}
