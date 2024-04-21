using System.ComponentModel.DataAnnotations;

namespace Leanwork.Rh.Application.DTO.JobInterviewWeight;

public class JobInterviewWeightDTO
{
    [Key]
    public int WeightInterviewVacancyId { get; set; } = 0;

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public int TechnologyId { get; set; } = 0;

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public int JobOpeningId { get; set; } = 0;

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public int Weight { get; set; } = 0;
}
