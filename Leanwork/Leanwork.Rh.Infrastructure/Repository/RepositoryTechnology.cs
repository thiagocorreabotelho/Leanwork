using Leanwork.Rh.Domain.Entitie;
using Leanwork.Rh.Domain.Interface;
using Leanwork.Rh.Infrastructure.Query;


namespace Leanwork.Rh.Infrastructure.Repository;

public class RepositoryTechnology : IRepositoryTechnology
{
    private readonly ISqlDataAccess _access;

    public RepositoryTechnology(ISqlDataAccess access)
    {
        _access = access;
    }

    /// <summary>
    /// Seleciona todos os registros da tabela de tecnologias.
    /// </summary>
    /// <returns>Uma coleção de objetos do tipo Technology representando os registros recuperados.</returns>
    public async Task<IEnumerable<Technology>> SelectAll()
    {
        var data = await _access.QueryAsync<Technology, dynamic>(QueryTechnology.SelectAll, new { });

        return data;
    }

    /// <summary>
    /// Seleciona uma tecnologia pelo seu ID.
    /// </summary>
    /// <param name="id">O ID da tecnologia a ser selecionada.</param>
    /// <returns>O objeto Technology correspondente ao ID fornecido, ou null se não encontrado.</returns>
    public async Task<Technology> SelectById(int id)
    {
        var parameters = new { Id = id };
        var data = await _access.QueryAsync<Technology, dynamic>(QueryTechnology.SelectById, parameters);

        return data.FirstOrDefault();
    }

    /// <summary>
    /// Insere uma nova tecnologia no banco de dados.
    /// </summary>
    /// <param name="technology">O objeto Technology a ser inserido.</param>
    /// <returns>O número de linhas afetadas pela operação de inserção.</returns>
    public async Task<int> Insert(Technology technology)
    {
        try
        {
            var parameters = new
            {
                technology.Name,
                technology.CreationDate,
                technology.ModificationDate,
            };

            // Executa o procedimento armazenado e captura o resultado
            var success = await _access.SaveData(QueryTechnology.Insert, parameters);

            // Retorna o resultado
            return success;
        }
        catch (Exception ex)
        {
            return 0; // Retorna falso em caso de erro
        }
    }

    /// <summary>
    /// Atualiza uma tecnologia existente no banco de dados.
    /// </summary>
    /// <param name="technology">O objeto Technology a ser atualizado.</param>
    /// <returns>O número de linhas afetadas pela operação de atualização.</returns>
    public async Task<int> Update(Technology technology)
    {
        try
        {
            var parameters = new
            {
                technology.TechnologyId,
                technology.Name,
                technology.CreationDate,
                technology.ModificationDate,
            };

            var success = await _access.SaveData(QueryTechnology.Update, parameters);

            return success;

        }
        catch (Exception ex)
        {
            return 0; // Retorna falso em caso de erro
        }
    }

    /// <summary>
    /// Exclui uma tecnologia do banco de dados com base no seu ID.
    /// </summary>
    /// <param name="id">O ID da tecnologia a ser excluída.</param>
    /// <returns>O número de linhas afetadas pela operação de exclusão.</returns>
    public async Task<int> Delete(int id)
    {
        try
        {
            var parameters = new { TechnologyId = id };
            var success = await _access.SaveData(QueryTechnology.Delete, parameters);

            return success;
        }
        catch (Exception ex)
        {
            return 0; // Retorna falso em caso de erro
        }
    }
}
