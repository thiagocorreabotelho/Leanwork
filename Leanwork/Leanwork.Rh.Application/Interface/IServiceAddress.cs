namespace Leanwork.Rh.Application;

public interface IServiceAddress
{
    Task<int> DeleteAsync(int id);
    Task<int> DeleteAllCandidateAsync(int id);
    Task<int> DeleteAllCompanyAsync(int id);
    Task<int> InsertAsync(AddressDTO addressDTO);
    Task<IEnumerable<AddressDTO>> SelectAllByCompanyAsync(int id);
    Task<IEnumerable<AddressDTO>> SelectAllByCandidateAsync(int id);
    Task<AddressDTO> SelectByIdAsync(int id);
    Task<int> UpdateAsync(AddressDTO addressDTO);
}
