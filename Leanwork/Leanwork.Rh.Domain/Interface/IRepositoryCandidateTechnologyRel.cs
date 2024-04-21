
using Leanwork.Rh.Domain;

public interface IRepositoryCandidateTechnologyRel
{
    Task<IEnumerable<CandidateTechnologyRel>> SelectAllTechnologiesByCandidate(int id);
    Task<int> Delete(int id);
    Task<int> Insert(CandidateTechnologyRel candidateTechnologyRel);
}
