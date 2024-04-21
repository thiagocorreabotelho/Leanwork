using AutoMapper;

using Leanwork.Rh.Application.Interface;
using Leanwork.Rh.Domain.Interface;
using Leanwork.Rh.Domain;
using Leanwork.Rh.Infrastructure;
using Leanwork.Rh.Application.DTO.ReportCandidate;

namespace Leanwork.Rh.Application.Service;

public class ServiceReportCandidate : ServiceBase, IServiceReportCandidate
{
    private IRepositoryReportCandidate _iRepositoryReportCandidate;
    private IMapper _iMapper;

    public ServiceReportCandidate(INotificationError notificationError, IMapper iMapper, IRepositoryReportCandidate iRepositoryReportCandidate) : base(notificationError)
    {
        _iMapper = iMapper;
        _iRepositoryReportCandidate = iRepositoryReportCandidate;
    }

    /// <summary>
    /// Recupera uma lista de candidatos para um relatório e mapeia os dados para DTOs.
    /// </summary>
    /// <returns>
    /// Uma tarefa que, quando executada, retorna uma coleção enumerável de <see cref="ReportCandidateDTO"/>,
    /// contendo os dados de cada candidato formatados como DTOs. Esta coleção é adequada para uso na camada de apresentação ou APIs.
    /// </returns>
    /// <remarks>
    /// Este método executa uma operação assíncrona para recuperar dados de relatório de candidatos usando o serviço <c>_iServiceReportCandidate</c>.
    /// Após a recuperação dos dados, ele utiliza o <c>_iMapper</c> para converter os dados de modelo de domínio para DTOs.
    /// Essa abordagem facilita a manipulação e transferência dos dados na camada de apresentação ou em outras partes do sistema
    /// que consomem esses dados de forma desacoplada do modelo de domínio direto.
    /// </remarks>
    public async Task<IEnumerable<ReportCandidateDTO>> SelectReportCandidate()
    {
        var report = await _iRepositoryReportCandidate.SelectReportCandidate();
        return _iMapper.Map<IEnumerable<ReportCandidateDTO>>(report);
    }
}
