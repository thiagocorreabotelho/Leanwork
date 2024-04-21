using Leanwork.Rh.Domain.Entitie;

namespace Leanwork.Rh.Domain.Interface;

public interface IRepositoryTechnology
{
    Task<int> Delete(int id);
    Task<int> Insert(Technology technology);
    Task<IEnumerable<Technology>> SelectAll();
    Task<Technology> SelectById(int id);
    Task<int> Update(Technology technology);
}
