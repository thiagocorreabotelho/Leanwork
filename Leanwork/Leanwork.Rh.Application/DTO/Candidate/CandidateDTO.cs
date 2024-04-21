using System.ComponentModel.DataAnnotations;
using Leanwork.Rh.Application.DTO.Technology;
using Leanwork.Rh.Domain.Entitie;

namespace Leanwork.Rh.Application;

public class CandidateDTO
{
    [Key]
    public int CandidateId { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public int CompanyId { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public int GenderId { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public string CPF { get; set; }

    public string RG { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public DateTime DateOfBirth  { get; set; }

     public List<AddressDTO> ListAddress {get; set;} = new List<AddressDTO>();
     public List<TechnologyDTO> ListTechnologies {get; set; } = new List<TechnologyDTO>();
}
