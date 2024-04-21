namespace Leanwork.Rh.Application;

public interface IServiceCandidate
{
     Task<int> DeleteAsync(int id);
    Task<int> InsertAsync(CandidateDTO candidateDTO);
    Task<IEnumerable<CandidateDTO>> SelectAllAsync();
    Task<CandidateDTO> SelectByIdAsync(int id);
    Task<int> UpdateAsync(CandidateDTO candidateDTO);
}
