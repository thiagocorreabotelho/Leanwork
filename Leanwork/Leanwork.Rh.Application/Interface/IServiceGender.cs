
namespace Leanwork.Rh.Application;

public interface IServiceGender
{
    Task<int> DeleteAsync(int id);
    Task<int> InsertAsync(GenderDTO genderDTO);
    Task<IEnumerable<GenderDTO>> SelectAllAsync();
    Task<GenderDTO> SelectByIdAsync(int id);
    Task<int> UpdateAsync(GenderDTO genderDTO);
}
