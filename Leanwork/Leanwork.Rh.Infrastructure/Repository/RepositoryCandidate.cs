using Leanwork.Rh.Domain;
using Leanwork.Rh.Domain.Interface;

namespace Leanwork.Rh.Infrastructure;

public class RepositoryCandidate : IRepositoryCandidate
{
    private readonly ISqlDataAccess _access;

    public RepositoryCandidate(ISqlDataAccess access)
    {
        _access = access;
    }


    /// <summary>
    /// Recupera de forma assíncrona todos os candidatos do banco de dados.
    /// </summary>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e contém um enumerável de objetos <see cref="Candidate"/>
    /// ao ser completada. Cada objeto representa um candidato recuperado do banco de dados.
    /// </returns>
    /// <remarks>
    /// Este método executa uma consulta para buscar todas as entradas da tabela de candidatos no banco de dados.
    /// Utiliza um objeto dinâmico para os parâmetros da consulta, que neste caso são vazios, pois a consulta
    /// não requer parâmetros específicos. O método aproveita o método QueryAsync do Dapper para realizar
    /// a operação de banco de dados de forma assíncrona.
    /// </remarks>
    public async Task<IEnumerable<Candidate>> SelectAll()
    {
        var data = await _access.QueryAsync<Candidate, dynamic>(QueryCandidate.SelectAll, new { });

        return data;
    }

    /// <summary>
    /// Recupera de forma assíncrona um candidato específico do banco de dados usando seu identificador.
    /// </summary>
    /// <param name="id">O identificador do candidato a ser recuperado.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e contém o objeto <see cref="Candidate"/>
    /// quando completada. Retorna o candidato correspondente ao identificador fornecido, ou null se nenhum candidato for encontrado.
    /// </returns>
    /// <remarks>
    /// Este método executa uma consulta para buscar um candidato específico pela sua coluna de identificação no banco de dados.
    /// Utiliza um objeto dinâmico para passar o parâmetro 'Id' para a consulta. O método utiliza o método QueryAsync do Dapper
    /// para realizar a operação de banco de dados de forma assíncrona, retornando o primeiro objeto encontrado ou null se nenhum
    /// objeto corresponder ao critério de busca.
    /// </remarks>
    public async Task<Candidate> SelectById(int id)
    {
        var parameters = new { Id = id };
        var data = await _access.QueryAsync<Candidate, dynamic>(QueryCandidate.SelectById, parameters);

        return data.FirstOrDefault();
    }

    /// <summary>
    /// Insere um novo candidato no banco de dados de forma assíncrona.
    /// </summary>
    /// <param name="candidate">O objeto <see cref="Candidate"/> que contém os dados do candidato a ser inserido.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e retorna um inteiro indicando o sucesso da operação.
    /// Retorna um valor maior que zero se a inserção for bem-sucedida, ou zero se ocorrer um erro.
    /// </returns>
    /// <remarks>
    /// Este método configura os parâmetros do candidato para passá-los a um procedimento armazenado que realiza a inserção.
    /// Os dados do candidato incluem informações pessoais e datas de criação e modificação. Em caso de sucesso, o procedimento
    /// armazenado retorna o número de registros afetados. Se uma exceção é capturada durante a operação, o método retorna zero,
    /// indicando que a inserção falhou.
    /// </remarks>
    public async Task<int> Insert(Candidate candidate)
    {
        try
        {
            var parameters = new
            {
                candidate.CompanyId,
                candidate.GenderId,
                candidate.FirstName,
                candidate.LastName,
                candidate.CPF,
                candidate.RG,
                candidate.DateOfBirth,
                candidate.CreationDate,
                candidate.ModificationDate,
            };

            // Executa o procedimento armazenado e captura o resultado
            var success = await _access.SaveData(QueryCandidate.Insert, parameters);

            // Retorna o resultado
            return success;
        }
        catch (Exception ex)
        {
            return 0; // Retorna falso em caso de erro
        }
    }

    /// <summary>
    /// Atualiza os dados de um candidato existente no banco de dados de forma assíncrona.
    /// </summary>
    /// <param name="candidate">O objeto <see cref="Candidate"/> que contém os dados atualizados do candidato.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e retorna um inteiro indicando o sucesso da operação.
    /// Retorna um valor maior que zero se a atualização for bem-sucedida, ou zero se ocorrer um erro.
    /// </returns>
    /// <remarks>
    /// Este método prepara os dados do candidato para serem atualizados, configurando parâmetros como identificador do candidato,
    /// nome, sobrenome, CPF, RG, data de nascimento e data de modificação. Estes parâmetros são passados a um procedimento armazenado
    /// responsável pela atualização. A função 'SaveData' é usada para executar este procedimento armazenado e capturar o número de registros
    /// afetados. Em caso de exceção durante a execução, o método captura a exceção e retorna zero, indicando que a atualização falhou.
    /// </remarks>
    public async Task<int> Update(Candidate candidate)
    {
        try
        {
            var parameters = new
            {
                candidate.CandidateId,
                candidate.FirstName,
                candidate.LastName,
                candidate.CPF,
                candidate.RG,
                candidate.DateOfBirth,
                candidate.ModificationDate,
            };

            var success = await _access.SaveData(QueryCandidate.Update, parameters);

            return success;

        }
        catch (Exception ex)
        {
            return 0; // Retorna falso em caso de erro
        }
    }

    /// <summary>
    /// Exclui um candidato do banco de dados de forma assíncrona utilizando seu identificador.
    /// </summary>
    /// <param name="id">O identificador do candidato a ser excluído.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e retorna um inteiro indicando o sucesso da operação.
    /// Retorna um valor maior que zero se a exclusão for bem-sucedida, ou zero se ocorrer um erro.
    /// </returns>
    /// <remarks>
    /// Este método configura os parâmetros necessários para a exclusão, especificamente o identificador do candidato,
    /// e chama um procedimento armazenado que executa a exclusão no banco de dados. Utiliza o método 'SaveData' do objeto
    /// '_access' para realizar essa operação assíncrona e obter o número de registros afetados. Em caso de falha na operação,
    /// como uma exceção capturada, o método retorna zero, indicando que a exclusão não foi realizada.
    /// </remarks>
    public async Task<int> Delete(int id)
    {
        try
        {
            var parameters = new { CandidateId = id };
            var success = await _access.SaveData(QueryCandidate.Delete, parameters);

            return success;
        }
        catch (Exception ex)
        {
            return 0; // Retorna falso em caso de erro
        }
    }
}
