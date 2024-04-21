using System.ComponentModel.DataAnnotations;

namespace Leanwork.Rh.Application;

public class ResponsibilityDTO
{
    [Key]
    public int ResponsibilityId { get; set; } = 0;

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public int JobOpeningId { get; set; } = 0;

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public string Description { get; set; } = string.Empty;

}
