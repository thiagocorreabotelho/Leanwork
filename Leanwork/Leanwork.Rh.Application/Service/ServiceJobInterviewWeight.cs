using AutoMapper;

using Leanwork.Rh.Application.DTO.JobInterviewWeight;
using Leanwork.Rh.Application.DTO.Technology;
using Leanwork.Rh.Application.Interface;
using Leanwork.Rh.Domain.Entitie;
using Leanwork.Rh.Domain.Interface;
using Leanwork.Rh.Domain.Validation;
using Leanwork.Rh.Infrastructure.Repository;

namespace Leanwork.Rh.Application.Service;

public class ServiceJobInterviewWeight : ServiceBase, IServiceJobInterviewWeight
{
    IRepositoryJobInterviewWeight _iRepositoryJobInterviewWeight;
    IMapper _iMapper;

    public ServiceJobInterviewWeight(INotificationError notificationError, IMapper iMapper, IRepositoryJobInterviewWeight iRepositoryJobInterviewWeight) : base(notificationError)
    {
        _iRepositoryJobInterviewWeight = iRepositoryJobInterviewWeight;
        _iMapper = iMapper;
    }

    /// <summary>
    /// Insere um novo peso de entrevista de vaga no banco de dados a partir de um DTO.
    /// </summary>
    /// <param name="jobInterviewWeightDTO">O Data Transfer Object (DTO) contendo os dados do peso de entrevista de vaga.</param>
    /// <returns>
    /// Retorna o ID do peso de entrevista de vaga inserido se a operação for bem-sucedida; caso contrário, retorna 0.
    /// </returns>
    /// <remarks>
    /// Este método mapeia o JobInterviewWeightDTO para a entidade JobInterviewWeight usando um mapeador.
    /// Antes de inserir no banco de dados, ele valida o objeto JobInterviewWeight. Se a validação falhar,
    /// ou se a inserção no repositório retornar 0 (indicando falha na inserção), o método notifica o erro
    /// e retorna 0. Se a inserção for bem-sucedida, atualiza o JobInterviewWeightDTO com o ID gerado e o retorna.
    /// </remarks>
    /// <exception cref="Exception">
    /// Lança uma exceção se ocorrer um erro inesperado durante a execução do processo de inserção e
    /// notifica a mensagem de erro usando o método Notify.
    /// </exception>
    public async Task<int> Insert(JobInterviewWeightDTO jobInterviewWeightDTO)
    {
        try
        {
            var jobInterviewWeight = _iMapper.Map<JobInterviewWeight>(jobInterviewWeightDTO);
            if (!Validate(jobInterviewWeight)) return 0;

            var saved = await _iRepositoryJobInterviewWeight.Insert(jobInterviewWeight);
            if (saved == 0)
            {
                Notify(Resource.ErrorMessageSave);
                return 0;
            }

            jobInterviewWeightDTO.WeightInterviewVacancyId = jobInterviewWeight.WeightInterviewVacancyId;
            return jobInterviewWeightDTO.WeightInterviewVacancyId;
        }
        catch (Exception ex)
        {
            Notify(string.Format(Resource.ErrorMessageException, ex.Message));
            return 0;
        }
    }

    /// <summary>
    /// Deleta um registro de peso de entrevista de vaga pelo ID fornecido.
    /// </summary>
    /// <param name="id">O identificador do peso de entrevista de vaga a ser deletado.</param>
    /// <returns>
    /// Retorna 1 se o registro foi deletado com sucesso; caso contrário, retorna 0 se o registro não foi encontrado ou se ocorreu um erro.
    /// </returns>
    /// <remarks>
    /// Este método tenta deletar um registro específico utilizando um ID. Se o método Delete do repositório retornar 0, 
    /// isso indica que nenhum registro foi deletado (possivelmente porque o registro não foi encontrado), e uma mensagem de erro é notificada.
    /// Se uma exceção for lançada durante a operação de deleção, uma mensagem de erro é notificada e o método retorna 0.
    /// </remarks>
    /// <exception cref="Exception">
    /// Captura e notifica qualquer exceção que ocorra durante o processo de deleção, indicando um problema no acesso ao repositório ou na execução do comando de deleção.
    /// </exception>
    public async Task<int> Delete(int id)
    {
        try
        {
            var saved = await _iRepositoryJobInterviewWeight.Delete(id);

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
    /// Valida os dados de um objeto JobInterviewWeight utilizando uma estratégia de validação específica.
    /// </summary>
    /// <param name="jobInterviewWeight">O objeto JobInterviewWeight a ser validado.</param>
    /// <returns>
    /// Retorna true se o objeto passar todas as validações; caso contrário, retorna false.
    /// </returns>
    /// <remarks>
    /// Este método utiliza o método RunValidation, passando um novo objeto ValidationJobInterviewWeight
    /// e o objeto jobInterviewWeight como argumentos. O método RunValidation deve implementar a lógica
    /// específica para validar os campos do objeto JobInterviewWeight, retornando true se todos os critérios
    /// forem atendidos, e false caso algum critério de validação falhe.
    /// </remarks>
    private bool Validate(JobInterviewWeight jobInterviewWeight)
    {
        if (!RunValidation(new ValidationJobInterviewWeight(), jobInterviewWeight)) return false;

        return true;
    }
}
