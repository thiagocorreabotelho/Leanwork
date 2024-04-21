using System.ComponentModel.DataAnnotations;
using Leanwork.Rh.Application.DTO.Technology;

namespace Leanwork.Rh.Application;

public class CompanyDTO
{
    [Key]
    public int CompanyId { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public string CNPJ { get; set; } = string.Empty;

    public DateTime OpenDate {get; set;}

    public string Email {get; set;}

     public List<AddressDTO> ListAddress {get; set;} = new List<AddressDTO>();
     public List<TechnologyDTO> ListTechnologies {get; set;} = new List<TechnologyDTO>();
}
