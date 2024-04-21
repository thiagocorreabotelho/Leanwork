using Leanwork.Rh.Domain.Entitie;

namespace Leanwork.Rh.Domain.Interface;

public interface IRepositoryJobInterviewWeight
{
    Task<int> Delete(int id);
    Task<int> Insert(JobInterviewWeight jobInterviewWeight);
}
