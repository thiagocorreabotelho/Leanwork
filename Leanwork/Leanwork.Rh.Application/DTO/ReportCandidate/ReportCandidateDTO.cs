using System.ComponentModel.DataAnnotations;

namespace Leanwork.Rh.Application.DTO.ReportCandidate;

public class ReportCandidateDTO
{
    [Key]
    public int CandidateId { get; set; } = 0;
    public string FullName { get; set; } 
    public string JobTitle { get; set; } 
    public string TotalScore { get; set; }
    
}
