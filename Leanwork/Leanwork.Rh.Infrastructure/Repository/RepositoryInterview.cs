using Leanwork.Rh.Domain;
using Leanwork.Rh.Domain.Entitie;
using Leanwork.Rh.Domain.Interface;
using Leanwork.Rh.Infrastructure.Query;

namespace Leanwork.Rh.Infrastructure.Repository;

public class RepositoryInterview : IRepositoryInterview
{
    private readonly ISqlDataAccess _access;

    public RepositoryInterview(ISqlDataAccess access)
    {
        _access = access;
    }

    /// <summary>
    /// Insere os dados de uma entrevista no banco de dados utilizando um procedimento armazenado.
    /// </summary>
    /// <param name="interview">O objeto Interview contendo os dados da entrevista a serem inseridos.</param>
    /// <returns>
    /// Retorna o número de registros afetados pela operação de inserção. Em caso de falha, retorna 0.
    /// </returns>
    /// <remarks>
    /// Este método configura os parâmetros necessários a partir do objeto interview e chama o método SaveData
    /// para executar o procedimento armazenado especificado em QueryInterview.Insert.
    /// A execução é feita de forma assíncrona. Em caso de exceção durante a execução, o método captura a exceção
    /// e retorna 0, indicando falha na operação.
    /// </remarks>
    /// <exception cref="Exception">
    /// Lança uma exceção se ocorrer um erro inesperado durante a execução do procedimento armazenado.
    /// </exception>
    public async Task<int> Insert(Interview interview)
    {
        try
        {
            var parameters = new
            {
                interview.CandidateId,
                interview.JobOpeningId,
                interview.CreationDate,
                interview.ModificationDate
            };

            // Executa o procedimento armazenado e captura o resultado
            var success = await _access.SaveData(QueryInterview.Insert, parameters);

            // Retorna o resultado
            return success;
        }
        catch (Exception ex)
        {
            return 0; // Retorna falso em caso de erro
        }
    }

    /// <summary>
    /// Deleta uma entrevista específica do banco de dados.
    /// </summary>
    /// <param name="id">O identificador único da entrevista a ser deletada.</param>
    /// <returns>
    /// Retorna o número de registros afetados pela operação de deleção. Em caso de falha, retorna 0.
    /// </returns>
    /// <remarks>
    /// Este método configura um objeto anônimo com o ID da entrevista e chama o método SaveData
    /// para executar o procedimento armazenado especificado em QueryInterview.Delete.
    /// A operação é executada de forma assíncrona. Em caso de exceção durante a execução, o método captura a exceção
    /// e retorna 0, indicando falha na operação.
    /// </remarks>
    /// <exception cref="Exception">
    /// Lança uma exceção se ocorrer um erro inesperado durante a execução do procedimento armazenado.
    /// </exception>
    public async Task<int> Delete(int id)
    {
        try
        {
            var parameters = new { InterviewId = id };
            var success = await _access.SaveData(QueryInterview.Delete, parameters);

            return success;
        }
        catch (Exception ex)
        {
            return 0; // Retorna falso em caso de erro
        }
    }
}
