using System.ComponentModel.DataAnnotations;

namespace Leanwork.Rh.Application;

public class GenderDTO
{
    [Key]
    public int GenderId { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public string Name { get; set; } = string.Empty;
}
