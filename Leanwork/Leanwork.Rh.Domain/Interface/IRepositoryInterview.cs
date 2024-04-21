using Leanwork.Rh.Domain.Entitie;

namespace Leanwork.Rh.Domain.Interface;

public interface IRepositoryInterview
{
    Task<int> Delete(int id);
    Task<int> Insert(Interview interview);
}
