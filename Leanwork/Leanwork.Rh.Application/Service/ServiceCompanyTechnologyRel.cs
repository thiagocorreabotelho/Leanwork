using AutoMapper;
using Leanwork.Rh.Application.Service;
using Leanwork.Rh.Domain;
using Leanwork.Rh.Domain.Interface;

namespace Leanwork.Rh.Application;

public class ServiceCompanyTechnologyRel : ServiceBase, IServiceCompanyTechnologyRel
{
    private IRepositoryCompanyTechnologyRel _repositoryCompanyTechnologyRel;
    private IMapper _mapper;

    public ServiceCompanyTechnologyRel(INotificationError notificationError, IMapper mapper, IRepositoryCompanyTechnologyRel repositoryCompanyTechnologyRel) : base(notificationError)
    {
        _repositoryCompanyTechnologyRel = repositoryCompanyTechnologyRel;
        _mapper = mapper;
    }

    /// <summary>
    /// Seleciona e retorna todas as tecnologias associadas a uma empresa específica.
    /// </summary>
    /// <remarks>
    /// Este método consulta o repositório para obter todas as tecnologias relacionadas a uma empresa com base no seu identificador.
    /// A consulta retorna uma coleção de entidades que são mapeadas para uma lista de DTOs antes de serem retornadas ao chamador.
    /// </remarks>
    /// <param name="id">Identificador da empresa para a qual as tecnologias estão sendo solicitadas.</param>
    /// <returns>
    /// Retorna uma lista de DTOs representando as tecnologias associadas à empresa especificada.
    /// </returns>
    public async Task<IEnumerable<CompanyTechnologyRelDTO>> SelectAllTechnologiesByCompanyAsync(int id)
    {
        var companyTechnologyRel = await _repositoryCompanyTechnologyRel.SelectAllTechnologiesByCompany(id);
        return _mapper.Map<IEnumerable<CompanyTechnologyRelDTO>>(companyTechnologyRel);
    }


    /// <summary>
    /// Insere uma nova relação de tecnologia de empresa no repositório.
    /// </summary>
    /// <remarks>
    /// Este método mapeia um DTO para uma entidade de relação de tecnologia de empresa, valida essa entidade,
    /// e então tenta inseri-la no repositório. Se a entidade não passar na validação ou a inserção falhar,
    /// uma notificação de erro é enviada. Exceções são capturadas e tratadas internamente, enviando uma
    /// notificação com a mensagem de erro.
    /// </remarks>
    /// <param name="companyTechnologyRelDTO">DTO da relação de tecnologia de empresa a ser inserida.</param>
    /// <returns>
    /// Retorna o identificador da relação inserida se bem-sucedida; caso contrário, retorna 0.
    /// </returns>
    /// <exception cref="Exception">Captura qualquer exceção gerada durante o processo e notifica o erro.</exception>
    public async Task<int> InsertAsync(CompanyTechnologyRelDTO companyTechnologyRelDTO)
    {
        try
        {
            var companyTechnologyRel = _mapper.Map<CompanyTechnologyRel>(companyTechnologyRelDTO);
            if (!Validate(companyTechnologyRel)) return 0;

            var saved = await _repositoryCompanyTechnologyRel.Insert(companyTechnologyRel);
            if (saved == 0)
            {
                Notify(Resource.ErrorMessageSave);
                return 0;
            }

            companyTechnologyRelDTO.CompanyTechnologyRelId = companyTechnologyRel.CompanyTechnologyRelId;
            return companyTechnologyRelDTO.CompanyTechnologyRelId;
        }
        catch (Exception ex)
        {
            Notify(string.Format(Resource.ErrorMessageException, ex.Message));
            return 0;
        }
    }

    /// <summary>
    /// Exclui uma relação de tecnologia de empresa com base em seu identificador.
    /// </summary>
    /// <remarks>
    /// Este método tenta excluir uma relação de tecnologia de empresa a partir do seu identificador.
    /// Se a exclusão não afetar nenhum registro (ou seja, nenhum registro foi encontrado com o ID fornecido),
    /// uma notificação de erro é gerada e o método retorna 0.
    /// Exceções capturadas durante o processo de exclusão são tratadas notificando o erro específico ocorrido.
    /// </remarks>
    /// <param name="id">O identificador da relação de tecnologia de empresa a ser excluída.</param>
    /// <returns>
    /// Retorna 1 se a exclusão for bem-sucedida; caso contrário, retorna 0 se nenhum registro for excluído ou se ocorrer um erro.
    /// </returns>
    /// <exception cref="Exception">Captura qualquer exceção que ocorra durante o processo de exclusão e notifica a mensagem de erro específica.</exception>
    public async Task<int> DeleteAsync(int id)
    {
        try
        {

            var saved = await _repositoryCompanyTechnologyRel.Delete(id);

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
    /// Valida uma entidade de relação de tecnologia de empresa.
    /// </summary>
    /// <remarks>
    /// Este método realiza a validação de uma entidade de relação de tecnologia de empresa utilizando uma classe de validação específica.
    /// A validação é feita através de uma chamada ao método 'RunValidation', que processa a entidade com as regras definidas na classe 'ValidationCompanyTechnologyRel'.
    /// </remarks>
    /// <param name="companyTechnologyRel">A entidade de relação de tecnologia de empresa a ser validada.</param>
    /// <returns>
    /// Retorna verdadeiro se a entidade passar por todas as validações; caso contrário, retorna falso.
    /// </returns>
    private bool Validate(CompanyTechnologyRel companyTechnologyRel)
    {
        if (!RunValidation(new ValidationCompanyTechnologyRel(), companyTechnologyRel)) return false;

        return true;
    }
}
