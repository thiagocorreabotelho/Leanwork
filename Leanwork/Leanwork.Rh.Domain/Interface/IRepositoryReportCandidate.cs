using Leanwork.Rh.Domain.Entitie;

namespace Leanwork.Rh.Domain.Interface;

public interface IRepositoryReportCandidate
{
    Task<IEnumerable<ReportCandidate>> SelectReportCandidate();
}
