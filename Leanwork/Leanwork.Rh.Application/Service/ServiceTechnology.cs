using AutoMapper;

using Leanwork.Rh.Application.DTO.Technology;
using Leanwork.Rh.Application.Interface;
using Leanwork.Rh.Domain.Entitie;
using Leanwork.Rh.Domain.Interface;
using Leanwork.Rh.Domain.Validation;


namespace Leanwork.Rh.Application.Service;

public class ServiceTechnology : ServiceBase, IServiceTechnology
{
    private IRepositoryTechnology _repositoryTechnology;
    private IMapper _mapper;

    public ServiceTechnology(INotificationError notificationError, IMapper mapper, IRepositoryTechnology repositoryTechnology) : base(notificationError)
    {
        _repositoryTechnology = repositoryTechnology;
        _mapper = mapper;
    }

    /// <summary>
    /// Seleciona todas as tecnologias no formato de objetos DTO de tecnologia.
    /// </summary>
    /// <returns>Uma coleção de objetos DTO de tecnologia representando as tecnologias selecionadas.</returns>
    public async Task<IEnumerable<TechnologyDTO>> SelectAllAsync()
    {
        var technology = await _repositoryTechnology.SelectAll();
        return _mapper.Map<IEnumerable<TechnologyDTO>>(technology);
    }

    /// <summary>
    /// Seleciona uma tecnologia pelo seu ID no formato de objeto DTO de tecnologia.
    /// </summary>
    /// <param name="id">O ID da tecnologia a ser selecionada.</param>
    /// <returns>O objeto DTO de tecnologia correspondente ao ID fornecido.</returns>
    public async Task<TechnologyDTO> SelectByIdAsync(int id)
    {
        var technology = await _repositoryTechnology.SelectById(id);
        return _mapper.Map<TechnologyDTO>(technology);
    }

    /// <summary>
    /// Insere uma nova tecnologia no banco de dados a partir de um objeto DTO de tecnologia.
    /// </summary>
    /// <param name="technologyDTO">O objeto DTO de tecnologia a ser inserido.</param>
    /// <returns>True se a inserção for bem-sucedida, False caso contrário.</returns>
    public async Task<int> InsertAsync(TechnologyDTO technologyDTO)
    {
        try
        {
            var technology = _mapper.Map<Technology>(technologyDTO);
            if (!Validate(technology)) return 0;

            var saved = await _repositoryTechnology.Insert(technology);
            if (saved == 0)
            {
                Notify(Resource.ErrorMessageSave);
                return 0;
            }

            technologyDTO.TechnologyId = technology.TechnologyId;
            return technologyDTO.TechnologyId;
        }
        catch (Exception ex)
        {
            Notify(string.Format(Resource.ErrorMessageException, ex.Message));
            return 0;
        }
    }

    /// <summary>
    /// Atualiza uma tecnologia no banco de dados a partir de um objeto DTO de tecnologia.
    /// </summary>
    /// <param name="technologyDTO">O objeto DTO de tecnologia com as informações atualizadas.</param>
    /// <returns>True se a atualização for bem-sucedida, False caso contrário.</returns>
    public async Task<int> UpdateAsync(TechnologyDTO technologyDTO)
    {
        try
        {
            var technology = _mapper.Map<Technology>(technologyDTO);
            if (!Validate(technology)) return 0;

            var saved = await _repositoryTechnology.Update(technology);
            if (saved == 0)
            {
                Notify(Resource.ErrorMessageUpdate);
                return 0;
            }

            return technology.TechnologyId;
        }
        catch (Exception ex)
        {
            Notify(string.Format(Resource.ErrorMessageException, ex.Message));
            return 0;
        }
    }

    /// <summary>
    /// Exclui uma tecnologia do banco de dados com base no seu ID.
    /// </summary>
    /// <param name="id">O ID da tecnologia a ser excluída.</param>
    /// <returns>True se a exclusão for bem-sucedida, False caso contrário.</returns>
    public async Task<int> DeleteAsync(int id)
    {
        try
        {
            var technology = await _repositoryTechnology.SelectById(id);
            if (technology == null)
            {
                Notify(Resource.RecordNotFoundErrorMessage);
                return 0;
            }

            var saved = await _repositoryTechnology.Delete(id);

            if (saved == 0)
            {
                Notify(Resource.ErrorMessageDelete);
                return 0;
            }

            return 1;
        }
        catch (Exception ex)
        {
            Notify(string.Format(Resource.ErrorMessageException, ex.Message));
            return 0;
        }
    }

    /// <summary>
    /// Valida uma instância de objeto Technology.
    /// </summary>
    /// <param name="technology">A instância de objeto Technology a ser validada.</param>
    /// <returns>True se a tecnologia for válida, False caso contrário.</returns>
    private bool Validate(Technology technology)
    {
        if (!RunValidation(new ValidationTechnology(), technology)) return false;

        return true;
    }
}
