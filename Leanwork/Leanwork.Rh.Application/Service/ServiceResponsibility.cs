using AutoMapper;
using Leanwork.Rh.Application.Service;
using Leanwork.Rh.Domain;
using Leanwork.Rh.Domain.Interface;

namespace Leanwork.Rh.Application;

public class ServiceResponsibility : ServiceBase, IServiceReesponsibility
{
    private IRepositoryResponsibility _repositoryResponsibility;
    private IMapper _iMapper;

    public ServiceResponsibility(INotificationError notificationError, IMapper iMapper, IRepositoryResponsibility repositoryResponsibility) : base(notificationError)
    {
        _repositoryResponsibility = repositoryResponsibility;
        _iMapper = iMapper;
    }

    /// <summary>
    /// Obtém de forma assíncrona todos os objetos de Responsibility do repositório e os mapeia para uma lista de objetos DTO.
    /// </summary>
    /// <returns>
    /// Retorna uma tarefa que eventualmente resulta em uma lista de objetos <see cref="ResponsibilityDTO"/>, representando todos os objetos Responsibility recuperados e mapeados do repositório.
    /// </returns>
    /// <remarks>
    /// Este método chama o método SelectAll do repositório para obter todos os objetos Responsibility,
    /// e então usa o AutoMapper para converter esses objetos para o tipo DTO antes de retorná-los.
    /// </remarks>
    public async Task<IEnumerable<ResponsibilityDTO>> SelectAllAsync()
    {
        var responsibility = await _repositoryResponsibility.SelectAll();
        return _iMapper.Map<IEnumerable<ResponsibilityDTO>>(responsibility);
    }

    /// <summary>
    /// Obtém de forma assíncrona todos os objetos de Responsibility disponíveis para alocação de trabalhos do repositório e os mapeia para uma lista de objetos DTO.
    /// </summary>
    /// <returns>
    /// Retorna uma tarefa que eventualmente resulta em uma lista de objetos <see cref="ResponsibilityDTO"/>, representando todos os objetos Responsibility disponíveis para trabalho recuperados e mapeados do repositório.
    /// </returns>
    /// <remarks>
    /// Este método chama o método SelectAllJobAvailable do repositório para obter apenas os objetos Responsibility que estão atualmente disponíveis para novos trabalhos,
    /// e em seguida, utiliza o AutoMapper para converter esses objetos para o tipo DTO antes de retorná-los.
    /// Esta função é útil para filtrar responsabilidades que podem ser atribuídas a novas tarefas ou empregos.
    /// </remarks>
    public async Task<IEnumerable<ResponsibilityDTO>> SelectAllByJobOpening(int id)
    {
        var responsibility = await _repositoryResponsibility.SelectAllByJobOpening(id);
        return _iMapper.Map<IEnumerable<ResponsibilityDTO>>(responsibility);
    }

    /// <summary>
    /// Obtém de forma assíncrona um objeto de Responsibility com base no ID fornecido e mapeia para um objeto DTO.
    /// </summary>
    /// <param name="id">O ID do objeto Responsibility a ser recuperado.</param>
    /// <returns>
    /// Retorna uma tarefa que eventualmente resulta em um objeto <see cref="ResponsibilityDTO"/> que representa o objeto Responsibility recuperado.
    /// Se nenhum objeto for encontrado, retorna null.
    /// </returns>
    /// <remarks>
    /// Este método chama o método SelectById do repositório para obter um objeto Responsibility específico.
    /// O AutoMapper é utilizado para converter o objeto Responsibility para o tipo DTO antes de retorná-lo.
    /// É importante verificar se o retorno não é null antes de usá-lo na aplicação.
    /// </remarks>
    public async Task<ResponsibilityDTO> SelectByIdAsync(int id)
    {
        var responsibility = await _repositoryResponsibility.SelectById(id);
        return _iMapper.Map<ResponsibilityDTO>(responsibility);
    }

    /// <summary>
    /// Insere de forma assíncrona um novo objeto Responsibility no repositório com base no DTO fornecido.
    /// </summary>
    /// <param name="responsibilityDTO">O DTO de Responsibility que contém os dados a serem inseridos.</param>
    /// <returns>
    /// Retorna o ID do objeto Responsibility inserido se a operação for bem-sucedida; caso contrário, retorna 0.
    /// Uma falha pode ocorrer devido à validação de dados falhar ou um erro no processo de salvamento.
    /// </returns>
    /// <remarks>
    /// Este método primeiramente mapeia o DTO para um objeto de domínio usando AutoMapper, verifica a validade dos dados, e se válido,
    /// tenta inseri-lo no repositório. Se a inserção falhar, uma mensagem de erro é notificada.
    /// Em caso de exceções, uma mensagem de erro formatada é notificada, e o método retorna 0.
    /// </remarks>
    /// <exception cref="Exception">
    /// Lança uma exceção formatada com a mensagem de erro em caso de falhas no processo de inserção.
    /// </exception>
    public async Task<int> InsertAsync(ResponsibilityDTO responsibilityDTO)
    {
        try
        {
            var responsibility = _iMapper.Map<Responsibility>(responsibilityDTO);
            if (!Validate(responsibility)) return 0;

            var saved = await _repositoryResponsibility.Insert(responsibility);
            if (saved == 0)
            {
                Notify(Resource.ErrorMessageSave);
                return 0;
            }

            responsibilityDTO.ResponsibilityId = responsibility.ResponsibilityId;
            return responsibilityDTO.ResponsibilityId;
        }
        catch (Exception ex)
        {
            Notify(string.Format(Resource.ErrorMessageException, ex.Message));
            return 0;
        }
    }

    /// <summary>
    /// Atualiza de forma assíncrona um objeto Responsibility existente com base nos dados fornecidos no DTO.
    /// </summary>
    /// <param name="responsibilityDTO">O DTO de Responsibility contendo as atualizações a serem aplicadas ao objeto.</param>
    /// <returns>
    /// Retorna o ID do objeto Responsibility atualizado se a operação for bem-sucedida; retorna 0 se a atualização falhar.
    /// </returns>
    /// <remarks>
    /// Este método primeiro mapeia o DTO para um objeto Responsibility, verifica a validade do objeto através do método <see cref="Validate"/>,
    /// e então tenta atualizar o objeto no repositório. Se a validação ou a atualização falharem, a operação retorna 0.
    /// Exceções capturadas durante a operação são tratadas internamente e notificadas ao usuário.
    /// </remarks>
    /// <exception cref="Exception">Lança uma exceção se ocorrer um problema durante a operação de atualização.</exception>
    public async Task<int> UpdateAsync(ResponsibilityDTO RrsponsibilityDTO)
    {
        try
        {
            var responsibility = _iMapper.Map<Responsibility>(RrsponsibilityDTO);
            if (!Validate(responsibility)) return 0;

            var saved = await _repositoryResponsibility.Update(responsibility);
            if (saved == 0)
            {
                Notify(Resource.ErrorMessageUpdate);
                return 0;
            }

            return responsibility.ResponsibilityId;
        }
        catch (Exception ex)
        {
            Notify(string.Format(Resource.ErrorMessageException, ex.Message));
            return 0;
        }
    }

    /// <summary>
    /// Deleta de forma assíncrona um objeto Responsibility identificado pelo ID fornecido.
    /// </summary>
    /// <param name="id">O ID do objeto Responsibility a ser deletado.</param>
    /// <returns>
    /// Retorna 1 se o objeto foi deletado com sucesso; retorna 0 se a deleção falhou ou se o objeto não foi encontrado.
    /// </returns>
    /// <remarks>
    /// Este método primeiro tenta recuperar o objeto Responsibility com o ID especificado.
    /// Se o objeto não existir, uma notificação de erro é enviada e o método retorna 0.
    /// Se o objeto existir, ele tenta deletá-lo do repositório.
    /// Caso a deleção falhe, uma notificação de erro é enviada.
    /// Exceções durante a operação são capturadas e notificadas.
    /// </remarks>
    /// <exception cref="Exception">Lança uma exceção se ocorrer um erro durante o processo de deleção.</exception>
    public async Task<int> DeleteAsync(int id)
    {
        try
        {
            var responsibility = await _repositoryResponsibility.SelectById(id);
            if (responsibility == null)
            {
                Notify(Resource.RecordNotFoundErrorMessage);
                return 0;
            }

            var saved = await _repositoryResponsibility.Delete(id);

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
    /// Valida o objeto Responsibility utilizando uma regra de validação específica.
    /// </summary>
    /// <param name="responsibility">O objeto Responsibility a ser validado.</param>
    /// <returns>
    /// Retorna verdadeiro se o objeto passar em todas as validações; caso contrário, retorna falso.
    /// </returns>
    /// <remarks>
    /// Este método utiliza o validador <see cref="ValidationResponsability"/> para verificar se o objeto cumpre com todas as regras necessárias.
    /// Se qualquer uma das validações falhar, o método retornará falso imediatamente, indicando que o objeto não é válido.
    /// </remarks>
    private bool Validate(Responsibility responsibility)
    {
        if (!RunValidation(new ValidationResponsability(), responsibility)) return false;

        return true;
    }
}
