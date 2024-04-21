using AutoMapper;
using Leanwork.Rh.Application.Service;
using Leanwork.Rh.Domain;
using Leanwork.Rh.Domain.Interface;

namespace Leanwork.Rh.Application;

public class ServiceCandidateTechnologyRel : ServiceBase, IServiceCandidateTechnologyRel
{
    private IRepositoryCandidateTechnologyRel _repositoryCandidateTechnologyRel;
    private IMapper _mapper;

    public ServiceCandidateTechnologyRel(INotificationError notificationError, IMapper mapper, IRepositoryCandidateTechnologyRel repositoryCandidateTechnologyRel) : base(notificationError)
    {
        _repositoryCandidateTechnologyRel = repositoryCandidateTechnologyRel;
        _mapper = mapper;
    }

    /// <summary>
    /// Consulta todas as tecnologias associadas a um determinado candidato.
    /// Este método recupera os dados do repositório correspondente ao candidato especificado pelo ID e mapeia esses dados
    /// para uma coleção de DTOs (Data Transfer Objects) para facilitar a transferência e manipulação dos dados em outras camadas da aplicação.
    /// </summary>
    /// <param name="id">O ID do candidato para o qual as tecnologias serão consultadas.</param>
    /// <returns>
    /// Task<IEnumerable<CandidateTechnologyRelDTO>>: Uma tarefa que retorna uma coleção de CandidateTechnologyRelDTO,
    /// representando as tecnologias associadas ao candidato.
    /// </returns>
    /// <remarks>
    /// Utiliza um repositório para recuperar os dados e um mapeador para transformar os dados de entidade em DTOs,
    /// proporcionando assim uma separação clara entre a camada de acesso a dados e a camada de negócios.
    /// </remarks>
    public async Task<IEnumerable<CandidateTechnologyRelDTO>> SelectAllTechnologiesByCandidateAsync(int id)
    {
        var candidateTechnologyRel = await _repositoryCandidateTechnologyRel.SelectAllTechnologiesByCandidate(id);
        return _mapper.Map<IEnumerable<CandidateTechnologyRelDTO>>(candidateTechnologyRel);
    }

    /// <summary>
    /// Insere de forma assíncrona um novo relacionamento entre candidato e tecnologia no banco de dados, baseado no DTO fornecido.
    /// Este método mapeia o DTO para um modelo de entidade, valida esse modelo e, se válido, prossegue com a inserção no repositório.
    /// </summary>
    /// <param name="candidateTechnologyRelDTO">O DTO de CandidateTechnologyRel que contém os dados necessários para a inserção.</param>
    /// <returns>
    /// Task<int>: Uma tarefa que retorna o ID do relacionamento inserido se a inserção for bem-sucedida; retorna 0 em caso de falha.
    /// </returns>
    /// <exception cref="Exception">Captura e trata exceções que podem ocorrer durante o processo de mapeamento, validação ou inserção.</exception>
    /// <remarks>
    /// Antes de inserir, este método valida o objeto CandidateTechnologyRel mapeado usando o método Validate.
    /// Se a validação falhar ou a inserção no repositório não for bem-sucedida, o usuário é notificado com uma mensagem de erro.
    /// </remarks>
    public async Task<int> InsertAsync(CandidateTechnologyRelDTO candidateTechnologyRelDTO)
    {
        try
        {
            var candidateTechnologyRel = _mapper.Map<CandidateTechnologyRel>(candidateTechnologyRelDTO);
            if (!Validate(candidateTechnologyRel)) return 0;

            var saved = await _repositoryCandidateTechnologyRel.Insert(candidateTechnologyRel);
            if (saved == 0)
            {
                Notify(Resource.ErrorMessageSave);
                return 0;
            }

            candidateTechnologyRelDTO.CandidateTechnologyRelId = candidateTechnologyRel.CandidateTechnologyRelId;
            return candidateTechnologyRelDTO.CandidateTechnologyRelId;
        }
        catch (Exception ex)
        {
            Notify(string.Format(Resource.ErrorMessageException, ex.Message));
            return 0;
        }
    }

    /// <summary>
    /// Exclui de forma assíncrona um relacionamento entre candidato e tecnologia no banco de dados, baseado no ID fornecido.
    /// Este método realiza a exclusão através do repositório e verifica o resultado da operação.
    /// </summary>
    /// <param name="id">O ID do relacionamento entre candidato e tecnologia a ser excluído.</param>
    /// <returns>
    /// Task<int>: Uma tarefa que retorna 1 se a exclusão for bem-sucedida; retorna 0 em caso de falha ou se nenhum registro foi afetado.
    /// </returns>
    /// <exception cref="Exception">Captura e trata exceções que podem ocorrer durante a exclusão, notificando o erro através de uma mensagem.</exception>
    /// <remarks>
    /// Se o método do repositório retornar 0, indicando que nenhum registro foi afetado, uma mensagem de erro é enviada utilizando o método Notify.
    /// </remarks>
    public async Task<int> DeleteAsync(int id)
    {
        try
        {

            var saved = await _repositoryCandidateTechnologyRel.Delete(id);

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
    /// Valida as informações de um relacionamento entre candidato e tecnologia utilizando uma classe de validação específica.
    /// Este método verifica se o objeto CandidateTechnologyRel passado como argumento atende a todas as regras de negócio estabelecidas
    /// para a entidade.
    /// </summary>
    /// <param name="candidateTechnologyRel">O objeto CandidateTechnologyRel a ser validado.</param>
    /// <returns>
    /// bool: Retorna verdadeiro se o objeto passar em todas as validações definidas; caso contrário, retorna falso.
    /// </returns>
    /// <remarks>
    /// Utiliza a classe ValidationCandidateTechnology para executar as validações específicas. Este método é útil para garantir
    /// que os dados estão corretos antes de realizar operações de inserção ou atualização no banco de dados.
    /// </remarks>
    private bool Validate(CandidateTechnologyRel candidateTechnologyRel)
    {
        if (!RunValidation(new ValidationCandidateTechnology(), candidateTechnologyRel)) return false;

        return true;
    }

}
