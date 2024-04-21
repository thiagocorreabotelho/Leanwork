namespace Leanwork.Rh.Domain;

public interface IRepositoryCandidate
{
    Task<int> Delete(int id);
    Task<int> Insert(Candidate candidate);
    Task<IEnumerable<Candidate>> SelectAll();
    Task<Candidate> SelectById(int id);
    Task<int> Update(Candidate candidate);
}
