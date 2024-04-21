using System.ComponentModel.DataAnnotations;

namespace Leanwork.Rh.Application;

public class CandidateTechnologyRelDTO
{
    [Key]
    public int CandidateTechnologyRelId { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public int CandidateId { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public int TechnologyId { get; set; }

    public string Name { get; set; } = string.Empty;
}
