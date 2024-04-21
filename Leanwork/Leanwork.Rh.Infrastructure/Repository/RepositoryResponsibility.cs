using Leanwork.Rh.Domain;
using Leanwork.Rh.Domain.Interface;

namespace Leanwork.Rh.Infrastructure;

public class RepositoryResponsibility : IRepositoryResponsibility
{
    private readonly ISqlDataAccess _access;

    public RepositoryResponsibility(ISqlDataAccess access)
    {
        _access = access;
    }

    /// <summary>
    /// Recupera de forma assíncrona todas as responsabilidades registradas no banco de dados.
    /// </summary>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém um enumerável de objetos <see cref="Responsibility"/>.
    /// </returns>
    public async Task<IEnumerable<Responsibility>> SelectAll()
    {
        var data = await _access.QueryAsync<Responsibility, dynamic>(QueryResponsibility.SelectAll, new { });

        return data;
    }

    /// <summary>
    /// Seleciona de forma assíncrona todas as responsabilidades associadas a uma abertura de vaga específica, identificada pelo ID.
    /// </summary>
    /// <param name="id">O ID da abertura de vaga cujas responsabilidades serão recuperadas.</param>
    /// <returns>
    /// Retorna uma tarefa que resulta em uma coleção de objetos <see cref="Responsibility"/> que representam as responsabilidades associadas à vaga especificada.
    /// </returns>
    /// <remarks>
    /// Este método realiza uma consulta assíncrona utilizando o Dapper para extrair as responsabilidades de acordo com o ID da vaga fornecido.
    /// A consulta é parametrizada para aumentar a segurança e a eficiência na execução da busca no banco de dados.
    /// </remarks>
    public async Task<IEnumerable<Responsibility>> SelectAllByJobOpening(int id)
    {
        var parameters = new { Id = id };
        var data = await _access.QueryAsync<Responsibility, dynamic>(QueryResponsibility.SelectAllByJobOpening, parameters);

        return data;
    }

    /// <summary>
    /// Recupera de forma assíncrona uma responsabilidade específica pelo ID do banco de dados.
    /// </summary>
    /// <param name="id">O identificador único da responsabilidade a ser recuperada.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém o objeto <see cref="Responsibility"/> correspondente ao ID fornecido, ou null se nenhuma responsabilidade for encontrada com esse ID.
    /// </returns>
    public async Task<Responsibility> SelectById(int id)
    {
        var parameters = new { Id = id };
        var data = await _access.QueryAsync<Responsibility, dynamic>(QueryResponsibility.SelectById, parameters);

        return data.FirstOrDefault();
    }

    /// <summary>
    /// Insere uma nova responsabilidade no banco de dados de forma assíncrona.
    /// </summary>
    /// <param name="responsibility">O objeto de responsabilidade contendo os dados para inserção, incluindo descrição, data de criação e data de modificação.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona. O resultado da tarefa retorna o número de registros afetados pela operação de inserção.
    /// Retorna 1 se a inserção foi bem-sucedida, 0 se ocorreu uma exceção, indicando falha na operação.
    /// </returns>
    public async Task<int> Insert(Responsibility responsibility)
    {
        try
        {
            var parameters = new
            {
                responsibility.JobOpeningId,
                responsibility.Description,
                responsibility.CreationDate,
                responsibility.ModificationDate
            };

            // Executa o procedimento armazenado e captura o resultado
            var success = await _access.SaveData(QueryResponsibility.Insert, parameters);

            // Retorna o resultado
            return success;
        }
        catch (Exception ex)
        {
            return 0; // Retorna falso em caso de erro
        }
    }

    /// <summary>
    /// Atualiza os dados de uma responsabilidade existente no banco de dados de forma assíncrona.
    /// </summary>
    /// <param name="responsibility">O objeto de responsabilidade contendo os dados atualizados, incluindo o ID da responsabilidade, descrição e data de modificação.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona. O resultado da tarefa retorna o número de registros afetados pela operação de atualização.
    /// Retorna o número de registros atualizados se a operação for bem-sucedida, ou 0 se ocorrer uma exceção, indicando falha na operação.
    /// </returns>
    public async Task<int> Update(Responsibility responsibility)
    {
        try
        {
            var parameters = new
            {
                responsibility.ResponsibilityId,
                responsibility.Description,
                responsibility.ModificationDate
            };

            var success = await _access.SaveData(QueryResponsibility.Update, parameters);

            return success;

        }
        catch (Exception ex)
        {
            return 0; // Retorna falso em caso de erro
        }
    }

    /// <summary>
    /// Remove uma responsabilidade do banco de dados de forma assíncrona, utilizando o ID fornecido.
    /// </summary>
    /// <param name="id">O identificador único da responsabilidade a ser removida.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona. O resultado da tarefa retorna o número de registros afetados pela operação de exclusão.
    /// Retorna 1 se a exclusão foi bem-sucedida, 0 se ocorreu uma exceção, indicando falha na operação.
    /// </returns>
    public async Task<int> Delete(int id)
    {
        try
        {
            var parameters = new { JobOpeningId = id };
            var success = await _access.SaveData(QueryResponsibility.Delete, parameters);

            return success;
        }
        catch (Exception ex)
        {
            return 0; // Retorna falso em caso de erro
        }
    }

}
