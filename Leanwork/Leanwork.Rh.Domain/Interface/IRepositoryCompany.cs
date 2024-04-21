namespace Leanwork.Rh.Domain;

public interface IRepositoryCompany
{
    Task<int> Delete(int id);
    Task<int> Insert(Company company);
    Task<IEnumerable<Company>> SelectAll();
    Task<Company> SelectById(int id);
    Task<int> Update(Company company);
}
