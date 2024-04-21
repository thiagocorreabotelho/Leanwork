using Leanwork.Rh.Domain.Entitie;
using Leanwork.Rh.Domain.Interface;
using Leanwork.Rh.Infrastructure.Query;

namespace Leanwork.Rh.Infrastructure.Repository;

public class RepositoryJobInterviewWeight : IRepositoryJobInterviewWeight
{
    readonly ISqlDataAccess _access;
    public RepositoryJobInterviewWeight(ISqlDataAccess access)
    {
        _access = access;
    }

    /// <summary>
    /// Insere um novo peso de entrevista de trabalho no banco de dados.
    /// </summary>
    /// <param name="jobInterviewWeight">O objeto JobInterviewWeight contendo os dados do peso da entrevista para serem inseridos.</param>
    /// <returns>
    /// Retorna o número de registros afetados pela operação de inserção. Em caso de falha, retorna 0.
    /// </returns>
    /// <remarks>
    /// Este método prepara os dados a serem inseridos configurando os parâmetros necessários a partir do objeto jobInterviewWeight.
    /// Em seguida, ele executa um procedimento armazenado especificado em QueryJobInterviewWeight.Insert para inserir os dados no banco de dados.
    /// Se a inserção falhar por qualquer motivo, como uma exceção durante a execução, o método retorna 0.
    /// </remarks>
    /// <exception cref="Exception">
    /// Lança uma exceção se ocorrer um erro inesperado durante a execução do procedimento armazenado.
    /// </exception>
    public async Task<int> Insert(JobInterviewWeight jobInterviewWeight)
    {
        try
        {
            var parameters = new
            {
                jobInterviewWeight.TechnologyId,
                jobInterviewWeight.JobOpeningId,
                jobInterviewWeight.Weight,
                jobInterviewWeight.CreationDate,
                jobInterviewWeight.ModificationDate,
            };

            // Executa o procedimento armazenado e captura o resultado
            var success = await _access.SaveData(QueryJobInterviewWeight.Insert, parameters);

            // Retorna o resultado
            return success;
        }
        catch (Exception ex)
        {
            return 0; // Retorna falso em caso de erro
        }
    }

    /// <summary>
    /// Deleta um registro de peso de entrevista de vaga pelo ID especificado.
    /// </summary>
    /// <param name="id">O ID do peso de entrevista de vaga a ser deletado.</param>
    /// <returns>
    /// Retorna o número de registros afetados pela operação de deleção. Retorna 0 em caso de falha.
    /// </returns>
    /// <remarks>
    /// Este método configura um parâmetro com o ID fornecido e executa um procedimento armazenado
    /// para deletar o registro correspondente do banco de dados. O método captura e trata qualquer
    /// exceção que ocorra durante a execução, retornando 0 em caso de erro, indicando que nenhum
    /// registro foi deletado.
    /// </remarks>
    /// <exception cref="Exception">
    /// Lança uma exceção se ocorrer um erro inesperado durante a execução do procedimento armazenado.
    /// </exception>
    public async Task<int> Delete(int id)
    {
        try
        {
            var parameters = new { WeightInterviewVacancyId = id };
            var success = await _access.SaveData(QueryJobInterviewWeight.Delete, parameters);

            return success;
        }
        catch (Exception ex)
        {
            return 0; // Retorna falso em caso de erro
        }
    }
}
