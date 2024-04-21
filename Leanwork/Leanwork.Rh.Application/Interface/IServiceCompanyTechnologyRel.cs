namespace Leanwork.Rh.Application;

public interface IServiceCompanyTechnologyRel
{
     Task<IEnumerable<CompanyTechnologyRelDTO>> SelectAllTechnologiesByCompanyAsync(int id);
    Task<int> DeleteAsync(int id);
    Task<int> InsertAsync(CompanyTechnologyRelDTO companyTechnologyRelDTO);
}
