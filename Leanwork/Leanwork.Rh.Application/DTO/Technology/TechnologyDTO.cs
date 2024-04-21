using System.ComponentModel.DataAnnotations;

namespace Leanwork.Rh.Application.DTO.Technology;

public class TechnologyDTO
{
    [Key]
    public int TechnologyId { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public string Name { get; set; } = string.Empty;
}
