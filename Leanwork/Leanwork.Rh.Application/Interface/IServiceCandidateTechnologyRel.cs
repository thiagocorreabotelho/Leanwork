namespace Leanwork.Rh.Application;

public interface IServiceCandidateTechnologyRel
{
    Task<IEnumerable<CandidateTechnologyRelDTO>> SelectAllTechnologiesByCandidateAsync(int id);
    Task<int> DeleteAsync(int id);
    Task<int> InsertAsync(CandidateTechnologyRelDTO candidateTechnologyRelDTO);
}
