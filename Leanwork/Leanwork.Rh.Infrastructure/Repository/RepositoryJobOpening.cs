using Leanwork.Rh.Domain;
using Leanwork.Rh.Domain.Interface;

namespace Leanwork.Rh.Infrastructure;

public class RepositoryJobOpening : IRepositoryJobOpening
{
    private readonly ISqlDataAccess _access;

    public RepositoryJobOpening(ISqlDataAccess access)
    {
        _access = access;
    }

    /// <summary>
    /// Asynchronously retrieves all job openings from the database.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an enumerable of <see cref="JobOpening"/> objects,
    /// which represent the job openings retrieved from the database.
    /// </returns>
    public async Task<IEnumerable<JobOpening>> SelectAll()
    {
        var data = await _access.QueryAsync<JobOpening, dynamic>(QueryJobOpening.SelectAll, new { });

        return data;
    }

    /// <summary>
    /// Recupera de forma assíncrona todas as vagas de emprego disponíveis do banco de dados.
    /// </summary>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona. O valor da tarefa é uma coleção enumerável de <see cref="JobOpening"/>,
    /// que contém todas as vagas de emprego disponíveis.
    /// </returns>
    /// <remarks>
    /// Este método utiliza o método <c>QueryAsync</c> da instância <c>_access</c> para executar a consulta SQL definida em <c>QueryJobOpening.SelectAllJobAvailable</c>.
    /// A consulta não requer parâmetros adicionais.
    /// </remarks>
    public async Task<IEnumerable<JobOpening>> SelectAvailableAll()
    {
        var data = await _access.QueryAsync<JobOpening, dynamic>(QueryJobOpening.SelectAllJobAvailable, new { });
        return data;
    }

    /// <summary>
    /// Asynchronously retrieves a specific job opening from the database by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the job opening to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the <see cref="JobOpening"/> object corresponding to the specified ID, or null if no such job opening is found.
    /// </returns>
    public async Task<JobOpening> SelectById(int id)
    {
        var parameters = new { Id = id };
        var data = await _access.QueryAsync<JobOpening, dynamic>(QueryJobOpening.SelectById, parameters);

        return data.FirstOrDefault();
    }

    /// <summary>
    /// Asynchronously inserts a new job opening into the database.
    /// </summary>
    /// <param name="jobOpening">The job opening to insert. This object should contain all necessary fields such as ResponsibilityId, Title, Summary, Description, CreationDate, ModificationDate, and Avaliable status.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the number of records affected by the insert operation. 
    /// Returns 1 if the insertion was successful, 0 if an exception was caught, indicating the operation failed.
    /// </returns>
    /// <exception cref="Exception">An exception that captures any errors that occur during the execution of the database command.</exception>
    public async Task<int> Insert(JobOpening jobOpening)
    {
        try
        {
            var parameters = new
            {
                jobOpening.Title,
                jobOpening.Summary,
                jobOpening.Description,
                jobOpening.CreationDate,
                jobOpening.ModificationDate,
                jobOpening.Available
            };

            // Executa o procedimento armazenado e captura o resultado
            var success = await _access.SaveData(QueryJobOpening.Insert, parameters);

            // Retorna o resultado
            return success;
        }
        catch (Exception ex)
        {
            return 0; // Retorna falso em caso de erro
        }
    }

    /// <summary>
    /// Asynchronously updates an existing job opening in the database.
    /// </summary>
    /// <param name="jobOpening">The job opening object containing the updated data. It must include the JobOpeningId for identification, along with updated fields such as ResponsibilityId, Title, Summary, Description, Available status, and ModificationDate.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the number of records affected by the update operation.
    /// Returns the number of records updated if successful, or 0 if an exception occurred, indicating the operation failed.
    /// </returns>
    /// <exception cref="Exception">An exception that captures any errors that occur during the execution of the database command.</exception>
    public async Task<int> Update(JobOpening jobOpening)
    {
        try
        {
            var parameters = new
            {
                jobOpening.JobOpeningId,
                jobOpening.Title,
                jobOpening.Summary,
                jobOpening.Description,
                jobOpening.Available,
                jobOpening.ModificationDate
            };

            var success = await _access.SaveData(QueryJobOpening.Update, parameters);

            return success;

        }
        catch (Exception ex)
        {
            return 0; // Retorna falso em caso de erro
        }
    }

    /// <summary>
    /// Asynchronously deletes a job opening from the database based on the provided ID.
    /// </summary>
    /// <param name="id">The unique identifier of the job opening to be deleted.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the number of records affected by the delete operation.
    /// Returns the number of records deleted if successful, or 0 if an exception occurred, indicating the operation failed.
    /// </returns>
    /// <exception cref="Exception">An exception that captures any errors that occur during the execution of the database command.</exception>
    public async Task<int> Delete(int id)
    {
        try
        {
            var parameters = new { JobOpeningId = id };
            var success = await _access.SaveData(QueryJobOpening.Delete, parameters);

            return success;
        }
        catch (Exception ex)
        {
            return 0; // Retorna falso em caso de erro
        }
    }
}
