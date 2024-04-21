namespace Leanwork.Rh.Domain;

public interface IRepositoryJobOpening
{
    Task<int> Delete(int id);
    Task<int> Insert(JobOpening jobOpening);
    Task<IEnumerable<JobOpening>> SelectAll();
    Task<IEnumerable<JobOpening>> SelectAvailableAll();
    Task<JobOpening> SelectById(int id);
    Task<int> Update(JobOpening jobOpening);
}
