namespace Leanwork.Rh.Application;

public interface IServiceReesponsibility
{
     Task<int> DeleteAsync(int id);
    Task<int> InsertAsync(ResponsibilityDTO responsibilityDTO);
    Task<IEnumerable<ResponsibilityDTO>> SelectAllAsync();
    Task<IEnumerable<ResponsibilityDTO>> SelectAllByJobOpening(int id);
    Task<ResponsibilityDTO> SelectByIdAsync(int id);
    Task<int> UpdateAsync(ResponsibilityDTO responsibilityDTO);
}
