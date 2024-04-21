namespace Leanwork.Rh.Domain;

public interface IRepositoryCompanyTechnologyRel
{
    Task<IEnumerable<CompanyTechnologyRel>> SelectAllTechnologiesByCompany(int id);
    Task<int> Delete(int id);
    Task<int> Insert(CompanyTechnologyRel companyTechnologyRel);
}
