using AutoMapper;
using Leanwork.Rh.Application.DTO.Interview;
using Leanwork.Rh.Application.Interface;
using Leanwork.Rh.Domain.Entitie;
using Leanwork.Rh.Domain.Interface;
using Leanwork.Rh.Domain.Validation;

namespace Leanwork.Rh.Application.Service;

public class ServiceInterview : ServiceBase, IServiceInterview
{
    private IRepositoryInterview _iRepositoryInterview;
    private IMapper _iMapper;

    public ServiceInterview(INotificationError notificationError, IMapper iMapper, IRepositoryInterview iRepositoryInterview) : base(notificationError)
    {
        _iRepositoryInterview = iRepositoryInterview;
        _iMapper = iMapper;
    }

    /// <summary>
    /// Insere de forma assíncrona uma entrevista no banco de dados após validação.
    /// </summary>
    /// <param name="interviewDTO">O Data Transfer Object (DTO) de Interview contendo os dados necessários para inserção.</param>
    /// <returns>
    /// O ID da entrevista inserida se a operação for bem-sucedida; caso contrário, retorna 0.
    /// </returns>
    /// <remarks>
    /// Este método mapeia o InterviewDTO para um objeto Interview usando o IMapper.
    /// Antes de inserir no banco de dados, ele valida o objeto Interview. Se a validação falhar,
    /// ou se a inserção no repositório retornar 0 (indicando falha na inserção), o método notifica o erro
    /// e retorna 0. Se a inserção for bem-sucedida, atualiza o InterviewDTO com o ID gerado e o retorna.
    /// </remarks>
    /// <exception cref="Exception">
    /// Captura qualquer exceção lançada durante o processo de mapeamento, validação ou inserção e
    /// notifica a mensagem de erro. Retorna 0 se uma exceção ocorrer.
    /// </exception>
    public async Task<int> Insert(InterviewDTO interviewDTO)
    {
        try
        {
            var interview = _iMapper.Map<Interview>(interviewDTO);
            if (!Validate(interview)) return 0;

            var saved = await _iRepositoryInterview.Insert(interview);
            if (saved == 0)
            {
                Notify(Resource.ErrorMessageSave);
                return 0;
            }

            interviewDTO.InterviewId = interview.InterviewId;
            return interviewDTO.InterviewId;
        }
        catch (Exception ex)
        {
            Notify(string.Format(Resource.ErrorMessageException, ex.Message));
            return 0;
        }
    }

    /// <summary>
    /// Deleta uma entrevista de forma assíncrona pelo ID fornecido.
    /// </summary>
    /// <param name="id">O ID da entrevista a ser deletada.</param>
    /// <returns>
    /// Retorna 1 se a entrevista for deletada com sucesso; caso contrário, retorna 0.
    /// </returns>
    /// <remarks>
    /// Este método tenta deletar uma entrevista usando o método Delete do repositório _iRepositoryInterview.
    /// Se o método do repositório retornar 0, isso indica que a entrevista não foi deletada (possivelmente porque não foi encontrada),
    /// e uma notificação de erro é disparada. O método retorna 1 se a exclusão for bem-sucedida, e 0 em caso de falha na exclusão ou exceção.
    /// </remarks>
    /// <exception cref="Exception">
    /// Captura e notifica qualquer exceção que ocorra durante o processo de deleção. Retorna 0 se ocorrer alguma exceção.
    /// </exception>
    public async Task<int> Delete(int id)
    {
        try
        {
            var saved = await _iRepositoryInterview.Delete(id);

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
    /// Valida os dados de uma entrevista utilizando uma estratégia de validação específica.
    /// </summary>
    /// <param name="interview">A instância de Interview a ser validada.</param>
    /// <returns>
    /// Retorna true se a entrevista passar por todas as validações; caso contrário, retorna false.
    /// </returns>
    /// <remarks>
    /// Este método utiliza o método RunValidation, passando um novo objeto ValidationInterview
    /// e o objeto interview como argumentos. O método RunValidation deve implementar a lógica
    /// específica para validar os campos do objeto Interview, retornando true se todos os critérios
    /// forem atendidos, e false caso algum critério de validação falhe.
    /// </remarks>
    private bool Validate(Interview interview)
    {
        if (!RunValidation(new ValidationInterview(), interview)) return false;

        return true;
    }
}
