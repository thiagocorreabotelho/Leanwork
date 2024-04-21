using Leanwork.Rh.Application.DTO.Technology;

namespace Leanwork.Rh.Application.Interface;

public interface IServiceTechnology
{
    Task<int> DeleteAsync(int id);
    Task<int> InsertAsync(TechnologyDTO technologyDTO);
    Task<IEnumerable<TechnologyDTO>> SelectAllAsync();
    Task<TechnologyDTO> SelectByIdAsync(int id);
    Task<int> UpdateAsync(TechnologyDTO technologyDTO);
}
