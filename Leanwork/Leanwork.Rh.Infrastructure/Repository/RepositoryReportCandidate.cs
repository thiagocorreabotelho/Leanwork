using Leanwork.Rh.Domain.Entitie;
using Leanwork.Rh.Domain.Interface;
using Leanwork.Rh.Infrastructure.Query;

namespace Leanwork.Rh.Infrastructure.Repository;

public class RepositoryReportCandidate : IRepositoryReportCandidate
{
    readonly ISqlDataAccess _access;

    public RepositoryReportCandidate(ISqlDataAccess access)
    {
        _access = access;
    }

    /// <summary>
    /// Recupera uma lista de candidatos para um relatório, incluindo informações detalhadas como pontuação em tecnologias para vagas específicas.
    /// </summary>
    /// <returns>
    /// Uma tarefa que, quando executada, retorna uma coleção enumerável de <see cref="ReportCandidate"/>, 
    /// contendo os dados de cada candidato conforme necessário para o relatório.
    /// </returns>
    /// <remarks>
    /// Este método executa uma consulta assíncrona no banco de dados usando o método <c>QueryAsync</c> da instância <c>_access</c>.
    /// A consulta específica que é executada é definida em <c>QueryReportCandidate.SelectReportCandidate</c>, e é projetada
    /// para reunir informações complexas necessárias para compor o relatório de candidatos, como pontuações de tecnologias e outras métricas relevantes.
    /// </remarks>
    public async Task<IEnumerable<ReportCandidate>> SelectReportCandidate()
    {
        var data = await _access.QueryAsync<ReportCandidate, dynamic>(QueryReportCandidate.SelectReportCandidate, new { });

        return data;
    }
}
