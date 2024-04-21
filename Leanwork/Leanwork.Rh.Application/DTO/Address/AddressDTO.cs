using System.ComponentModel.DataAnnotations;

namespace Leanwork.Rh.Application;

public class AddressDTO
{
    [Key]
    public int AddressId { get; set; }

    public int CompanyId { get; set; }

    public int CandidateId { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public string ZipCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public string Street { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public string Number { get; set; } = string.Empty;

    public string Complement { get; set; } = null!;

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public string Neighborhood { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public string State { get; set; } = string.Empty;
}
