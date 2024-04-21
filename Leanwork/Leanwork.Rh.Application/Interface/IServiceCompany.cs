namespace Leanwork.Rh.Application;

public interface IServiceCompany
{
    Task<int> DeleteAsync(int id);
    Task<int> InsertAsync(CompanyDTO companyDTO);
    Task<IEnumerable<CompanyDTO>> SelectAllAsync();
    Task<CompanyDTO> SelectByIdAsync(int id);
    Task<int> UpdateAsync(CompanyDTO companyDTO);
}
