using System.ComponentModel.DataAnnotations;

namespace Leanwork.Rh.Application;

public class JobOpeningDTO
{
    [Key]
    public int JobOpeningId { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public string Summary { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public bool Available { get; set; } = false;

    public List<ResponsibilityDTO> ListResponsibility { get; set; } = new List<ResponsibilityDTO>();
}
