using Leanwork.Rh.Application.DTO.ReportCandidate;

namespace Leanwork.Rh.Application.Interface;

public interface IServiceReportCandidate
{
    Task<IEnumerable<ReportCandidateDTO>> SelectReportCandidate();
}
