using System.ComponentModel.DataAnnotations;

namespace Leanwork.Rh.Application;

public class CompanyTechnologyRelDTO
{
    [Key]
    public int CompanyTechnologyRelId { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public int CompanyId { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public int TechnologyId { get; set; }

    public string Name { get; set; } = string.Empty;
}
