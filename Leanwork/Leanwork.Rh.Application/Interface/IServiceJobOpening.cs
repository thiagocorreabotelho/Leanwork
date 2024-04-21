namespace Leanwork.Rh.Application;

public interface IServiceJobOpening
{
     Task<int> DeleteAsync(int id);
    Task<int> InsertAsync(JobOpeningDTO jobOpeningDTO);
    Task<IEnumerable<JobOpeningDTO>> SelectAllAsync();
    Task<IEnumerable<JobOpeningDTO>> SelectAllAvailableAsync();
    Task<JobOpeningDTO> SelectByIdAsync(int id);
    Task<int> UpdateAsync(JobOpeningDTO jobOpeningDTO);
}
