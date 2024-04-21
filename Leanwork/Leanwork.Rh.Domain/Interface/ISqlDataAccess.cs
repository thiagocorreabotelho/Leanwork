namespace Leanwork.Rh.Domain.Interface;

public interface ISqlDataAccess
{
    Task<IEnumerable<T>> QueryAsync<T, U>(string queryName, U parameters, string connectionName = "Default");
    Task<int> SaveData<T>(string queryName, T parameters, string connectionName = "Default");
}
