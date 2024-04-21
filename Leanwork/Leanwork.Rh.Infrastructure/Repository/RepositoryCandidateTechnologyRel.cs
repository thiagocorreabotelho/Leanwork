using Leanwork.Rh.Domain;
using Leanwork.Rh.Domain.Interface;

namespace Leanwork.Rh.Infrastructure;

public class RepositoryCandidateTechnologyRel : IRepositoryCandidateTechnologyRel
{
    private readonly ISqlDataAccess _access;

    public RepositoryCandidateTechnologyRel(ISqlDataAccess access)
    {
        _access = access;
    }

    /// <summary>
    /// Recupera todas as tecnologias associadas a um candidato específico.
    /// Este método executa uma consulta assíncrona no banco de dados utilizando um procedimento armazenado,
    /// que retorna todas as tecnologias vinculadas ao ID do candidato fornecido.
    /// </summary>
    /// <param name="id">O ID do candidato para o qual as tecnologias serão recuperadas.</param>
    /// <returns>
    /// Task<IEnumerable<Company>>: Uma tarefa que retorna uma coleção de objetos do tipo Company.
    /// Cada objeto Company representa uma tecnologia associada ao candidato.
    /// </returns>
    /// <exception cref="Exception">Captura e trata exceções que podem ocorrer durante a execução da consulta ao banco de dados.</exception>
    public async Task<IEnumerable<CandidateTechnologyRel>> SelectAllTechnologiesByCandidate(int id)
    {
        var parameters = new { Id = id };
        var data = await _access.QueryAsync<CandidateTechnologyRel, dynamic>(QueryCandidateTechnologyRel.SelectAllTechnologyByCandidate, parameters);

        return data;
    }

    /// <summary>
    /// Insere um relacionamento entre um candidato e uma tecnologia no banco de dados.
    /// Utiliza um procedimento armazenado para realizar a inserção e retorna um inteiro indicando o resultado da operação.
    /// </summary>
    /// <param name="candidateTechnologyRel">Objeto do tipo CandidateTechnologyRel contendo os detalhes do relacionamento,
    /// incluindo os identificadores do candidato e da tecnologia, e as datas de criação e modificação.</param>
    /// <returns>
    /// Task<int>: Retorna um inteiro de forma assíncrona. Se o procedimento for executado com sucesso, retorna o resultado obtido;
    /// em caso de exceção, retorna 0.
    /// </returns>
    /// <exception cref="Exception">Captura e trata exceções que ocorrem durante a execução do procedimento armazenado,
    /// retornando 0 em caso de falha.</exception>
    public async Task<int> Insert(CandidateTechnologyRel candidateTechnologyRel)
    {
        try
        {
            var parameters = new
            {
                candidateTechnologyRel.CandidateId,
                candidateTechnologyRel.TechnologyId,
                candidateTechnologyRel.CreationDate,
                candidateTechnologyRel.ModificationDate,
            };

            // Executa o procedimento armazenado e captura o resultado
            var success = await _access.SaveData(QueryCandidateTechnologyRel.Insert, parameters);

            // Retorna o resultado
            return success;
        }
        catch (Exception ex)
        {
            return 0; // Retorna falso em caso de erro
        }
    }

    /// <summary>
    /// Exclui um registro específico de relacionamento entre candidato e tecnologia no banco de dados, baseado no ID fornecido.
    /// Este método executa um procedimento armazenado de exclusão e retorna um inteiro indicando o sucesso da operação.
    /// </summary>
    /// <param name="id">O ID do relacionamento entre candidato e tecnologia a ser excluído.</param>
    /// <returns>
    /// Task<int>: Retorna um inteiro de forma assíncrona. Se a exclusão for bem-sucedida, retorna o número de registros afetados;
    /// em caso de exceção, retorna 0.
    /// </returns>
    /// <exception cref="Exception">Captura e trata exceções que podem ocorrer durante a execução do procedimento armazenado,
    /// retornando 0 em caso de falha.</exception>
    public async Task<int> Delete(int id)
    {
        try
        {
            var parameters = new { CandidateTechnologyId = id };
            var success = await _access.SaveData(QueryCandidateTechnologyRel.Delete, parameters);

            return success;
        }
        catch (Exception ex)
        {
            return 0; // Retorna falso em caso de erro
        }
    }

  
}
