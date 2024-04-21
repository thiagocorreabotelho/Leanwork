namespace Leanwork.Rh.Domain;

public interface IRepositoryResponsibility
{
    Task<int> Delete(int id);
    Task<int> Insert(Responsibility responsibility);
    Task<IEnumerable<Responsibility>> SelectAll();
    Task<IEnumerable<Responsibility>> SelectAllByJobOpening(int id);
    Task<Responsibility> SelectById(int id);
    Task<int> Update(Responsibility responsibility);
}