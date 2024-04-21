namespace Leanwork.Rh.Domain;

public interface IRepositoryGender
{
    Task<int> Delete(int id);
    Task<int> Insert(Gender gender);
    Task<IEnumerable<Gender>> SelectAll();
    Task<Gender> SelectById(int id);
    Task<int> Update(Gender gender);
}
