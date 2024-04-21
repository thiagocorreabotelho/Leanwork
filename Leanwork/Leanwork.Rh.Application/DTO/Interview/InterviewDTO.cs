using System.ComponentModel.DataAnnotations;

namespace Leanwork.Rh.Application.DTO.Interview;

public class InterviewDTO
{
    [Key]
    public int InterviewId { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public int CandidateId { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public int JobOpeningId { get; set; }
}
