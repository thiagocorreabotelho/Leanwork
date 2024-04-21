namespace Leanwork.Rh.Domain;

public interface IRepositoryAddress
{
    Task<int> Delete(int id);
    Task<int> DeleteAllCompanyAsync(int id);
    Task<int> DeleteAllCandidateAsync(int id);
    Task<int> Insert(Address address);
    Task<IEnumerable<Address>> SelectAllByCompany(int id);
    Task<IEnumerable<Address>> SelectAllByCandidate(int id);
    Task<Address> SelectById(int id);
    Task<int> Update(Address address);
}
