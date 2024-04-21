using Leanwork.Rh.Domain;
using Leanwork.Rh.Domain.Interface;

namespace Leanwork.Rh.Infrastructure;

public class RepositoryGender : IRepositoryGender
{
    private readonly ISqlDataAccess _access;

    public RepositoryGender(ISqlDataAccess access)
    {
        _access = access;
    }

    /// <summary>
    /// Recupera de forma assíncrona todos os registros de gênero do banco de dados.
    /// </summary>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e contém uma coleção enumerável de objetos <see cref="Gender"/>
    /// quando completada. Cada objeto representa um registro de gênero recuperado do banco de dados.
    /// </returns>
    /// <remarks>
    /// Este método executa uma consulta para buscar todos os registros da tabela de gêneros no banco de dados.
    /// Utiliza o método QueryAsync do Dapper para realizar a operação de banco de dados de forma assíncrona.
    /// Não são necessários parâmetros para a consulta, o que é indicado pelo uso de um objeto dinâmico vazio na chamada.
    /// </remarks>
    public async Task<IEnumerable<Gender>> SelectAll()
    {
        var data = await _access.QueryAsync<Gender, dynamic>(QueryGender.SelectAll, new { });

        return data;
    }

    /// <summary>
    /// Recupera de forma assíncrona um registro de gênero específico do banco de dados pelo seu identificador.
    /// </summary>
    /// <param name="id">O identificador do gênero a ser recuperado.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona, retornando o objeto <see cref="Gender"/> correspondente ao identificador fornecido,
    /// ou <c>null</c> se nenhum registro for encontrado.
    /// </returns>
    /// <remarks>
    /// Este método executa uma consulta ao banco de dados para buscar um registro de gênero específico pela coluna de identificação.
    /// Utiliza o método 'QueryAsync' do Dapper com parâmetros dinâmicos para especificar o ID desejado. A consulta é feita de forma
    /// assíncrona para melhorar a performance e evitar bloqueios da thread principal. O método retorna o primeiro objeto encontrado
    /// na consulta ou null se a consulta não retornar nenhum resultado.
    /// </remarks>
    public async Task<Gender> SelectById(int id)
    {
        var parameters = new { Id = id };
        var data = await _access.QueryAsync<Gender, dynamic>(QueryGender.SelectById, parameters);

        return data.FirstOrDefault();
    }

    /// <summary>
    /// Insere um novo registro de gênero no banco de dados de forma assíncrona.
    /// </summary>
    /// <param name="gender">O objeto <see cref="Gender"/> que contém os dados do gênero a ser inserido.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e retorna um inteiro indicando o sucesso da operação.
    /// Retorna o número de registros afetados, ou zero em caso de falha na inserção.
    /// </returns>
    /// <remarks>
    /// Este método prepara e executa uma operação de inserção no banco de dados utilizando um procedimento armazenado.
    /// Os parâmetros para a inserção, como o nome do gênero e as datas de criação e modificação, são encapsulados dentro de um
    /// objeto anônimo que é passado para o método 'SaveData'. A execução do procedimento armazenado é feita de forma assíncrona
    /// para evitar bloqueios da thread principal e melhorar a eficiência da aplicação. Em caso de exceção, como falha na conexão
    /// ou erro na execução do SQL, o método captura a exceção e retorna zero, indicando que a inserção falhou.
    /// </remarks>
    public async Task<int> Insert(Gender gender)
    {
        try
        {
            var parameters = new
            {
                gender.Name,
                gender.CreationDate,
                gender.ModificationDate,
            };

            // Executa o procedimento armazenado e captura o resultado
            var success = await _access.SaveData(QueryGender.Insert, parameters);

            // Retorna o resultado
            return success;
        }
        catch (Exception ex)
        {
            return 0; // Retorna falso em caso de erro
        }
    }

    /// <summary>
    /// Atualiza os dados de um registro de gênero existente no banco de dados de forma assíncrona.
    /// </summary>
    /// <param name="gender">O objeto <see cref="Gender"/> que contém os dados atualizados do gênero.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e retorna um inteiro indicando o sucesso da operação.
    /// Retorna o número de registros afetados, ou zero em caso de falha na atualização.
    /// </returns>
    /// <remarks>
    /// Este método prepara e executa uma operação de atualização no banco de dados utilizando um procedimento armazenado.
    /// Os parâmetros para a atualização, como o identificador do gênero, nome e data de modificação, são encapsulados dentro de um
    /// objeto anônimo que é passado para o método 'SaveData'. A execução do procedimento armazenado é feita de forma assíncrona
    /// para evitar bloqueios da thread principal e melhorar a eficiência da aplicação. Em caso de exceção, como falha na conexão
    /// ou erro na execução do SQL, o método captura a exceção e retorna zero, indicando que a atualização falhou.
    /// </remarks>
    public async Task<int> Update(Gender gender)
    {
        try
        {
            var parameters = new
            {
                gender.GenderId,
                gender.Name,
                gender.ModificationDate,
            };

            var success = await _access.SaveData(QueryGender.Update, parameters);

            return success;

        }
        catch (Exception ex)
        {
            return 0; // Retorna falso em caso de erro
        }
    }

    /// <summary>
    /// Exclui um registro de gênero existente no banco de dados de forma assíncrona, utilizando seu identificador.
    /// </summary>
    /// <param name="id">O identificador do gênero a ser excluído.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e retorna um inteiro indicando o sucesso da operação.
    /// Retorna o número de registros afetados pela exclusão, ou zero em caso de falha na operação.
    /// </returns>
    /// <remarks>
    /// Este método prepara e executa uma operação de exclusão no banco de dados utilizando um procedimento armazenado.
    /// O identificador do gênero é utilizado como parâmetro para especificar qual registro deve ser excluído.
    /// A execução do procedimento armazenado é feita de forma assíncrona para evitar bloqueios da thread principal e
    /// melhorar a eficiência da aplicação. Em caso de exceção, como falha na conexão ou erro na execução do SQL,
    /// o método captura a exceção e retorna zero, indicando que a exclusão falhou.
    /// </remarks>
    public async Task<int> Delete(int id)
    {
        try
        {
            var parameters = new { GenderId = id };
            var success = await _access.SaveData(QueryGender.Delete, parameters);

            return success;
        }
        catch (Exception ex)
        {
            return 0; // Retorna falso em caso de erro
        }
    }
}
