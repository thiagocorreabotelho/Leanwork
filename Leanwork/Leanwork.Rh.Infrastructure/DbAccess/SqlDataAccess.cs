using System.Data;
using System.Data.SqlClient;
using Leanwork.Rh.Domain.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Dapper;


namespace Leanwork.Rh.Infrastructure.DbAccess;

public class SqlDataAccess : DbContext, ISqlDataAccess
{
    private readonly IConfiguration _configuration;

    public SqlDataAccess(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Executa uma consulta assíncrona no banco de dados usando um procedimento armazenado.
    /// </summary>
    /// <typeparam name="T">O tipo de objetos retornados pela consulta.</typeparam>
    /// <typeparam name="U">O tipo dos parâmetros passados para o procedimento armazenado.</typeparam>
    /// <param name="queryName">O nome da query a ser executado.</param>
    /// <param name="parameters">Os parâmetros a serem passados para o procedimento armazenado.</param>
    /// <param name="connectionName">O nome da string de conexão a ser usada (o padrão é "DefaultConnection").</param>
    /// <returns>
    /// Uma operação assíncrona que retorna uma coleção de objetos do tipo <typeparamref name="T"/>.
    /// </returns>
    public async Task<IEnumerable<T>> QueryAsync<T, U>(string query, U parameters, string connectionName = "DefaultConnection")
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString(connectionName));
        var runQuery = await connection.QueryAsync<T>(query, parameters, commandType: CommandType.Text);
        return runQuery;
    }

    /// <summary>
    /// Executa um procedimento armazenado no banco de dados e retorna um valor indicando se a operação foi bem-sucedida.
    /// </summary>
    /// <typeparam name="T">O tipo de parâmetros para o procedimento armazenado.</typeparam>
    /// <param name="query">O nome da query a ser executado.</param>
    /// <param name="parameters">Os parâmetros a serem passados para o procedimento armazenado.</param>
    /// <param name="connectionName">O nome da conexão com o banco de dados (padrão é "Default").</param>
    /// <returns>True se a execução do procedimento armazenado foi bem-sucedida; caso contrário, false.</returns>
    public async Task<int> SaveData<T>(string query, T parameters, string connectionName = "Default")
    {
        try
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString(connectionName));

            var parametersWithOutput = new DynamicParameters(parameters);
            parametersWithOutput.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await connection.ExecuteAsync(query, parametersWithOutput, commandType: CommandType.Text);
            int id = parametersWithOutput.Get<int>("@Id");

            return id;
           
        }
        catch (Exception ex)
        {
            // Considerar logar o erro ou tratá-lo conforme necessário
            throw; // Recomendo lançar a exceção para entender melhor o que pode estar errado.
        }
    }

}
