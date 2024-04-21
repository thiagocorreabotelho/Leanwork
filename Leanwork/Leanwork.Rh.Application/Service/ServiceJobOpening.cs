using AutoMapper;
using Leanwork.Rh.Application.Service;
using Leanwork.Rh.Domain;
using Leanwork.Rh.Domain.Interface;

namespace Leanwork.Rh.Application;

public class ServiceJobOpening : ServiceBase, IServiceJobOpening
{
    private IServiceReesponsibility _iServiceResponsibility;
    private IRepositoryJobOpening _repositoryJobOpening;
    private IMapper _imapper;

    public ServiceJobOpening(INotificationError notificationError, IMapper imapper, IRepositoryJobOpening repositoryJobOpening, IServiceReesponsibility iServiceResponsibility) : base(notificationError)
    {
        _iServiceResponsibility = iServiceResponsibility;
        _repositoryJobOpening = repositoryJobOpening;
        _imapper = imapper;
    }

    /// <summary>
    /// Recupera de forma assíncrona todas as vagas de emprego disponíveis no repositório e as mapeia para uma coleção de DTOs de vagas de emprego.
    /// </summary>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma coleção de objetos <see cref="JobOpeningDTO"/> que representam as vagas de emprego.
    /// </returns>
    public async Task<IEnumerable<JobOpeningDTO>> SelectAllAsync()
    {
        var jobOpening = await _repositoryJobOpening.SelectAll();
        return _imapper.Map<IEnumerable<JobOpeningDTO>>(jobOpening);
    }

    /// <summary>
    /// Recupera de forma assíncrona todas as vagas de emprego disponíveis e as converte para DTOs.
    /// </summary>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona. O valor da tarefa é uma coleção enumerável de <see cref="JobOpeningDTO"/>,
    /// que representa as vagas de emprego disponíveis em forma de Data Transfer Objects (DTOs).
    /// </returns>
    /// <remarks>
    /// Este método inicialmente chama <c>SelectAvailableAll</c> do repositório <c>_repositoryJobOpening</c> para obter as vagas disponíveis.
    /// Após obter os dados, utiliza o <c>_imapper</c> para mapear a lista de entidades de <c>JobOpening</c> para <c>JobOpeningDTO</c>.
    /// </remarks>
    public async Task<IEnumerable<JobOpeningDTO>> SelectAllAvailableAsync()
    {
        var jobOpening = await _repositoryJobOpening.SelectAvailableAll();
        return _imapper.Map<IEnumerable<JobOpeningDTO>>(jobOpening);
    }

    /// <summary>
    /// Recupera de forma assíncrona uma vaga de emprego específica pelo seu ID a partir do repositório e a mapeia para um DTO de vaga de emprego.
    /// </summary>
    /// <param name="id">O identificador único da vaga de emprego a ser recuperada.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém um objeto <see cref="JobOpeningDTO"/> representando a vaga de emprego, ou null se nenhuma vaga for encontrada.
    /// </returns>
    public async Task<JobOpeningDTO> SelectByIdAsync(int id)
    {
        var jobOpening = await _repositoryJobOpening.SelectById(id);
        var jobOpeningDTO = _imapper.Map<JobOpeningDTO>(jobOpening);

        var responsibilityDTO = await _iServiceResponsibility.SelectAllByJobOpening(id);
        jobOpeningDTO.ListResponsibility = responsibilityDTO.ToList();

        return jobOpeningDTO;
    }

    /// <summary>
    /// Insere de forma assíncrona uma nova vaga de emprego no repositório após mapear e validar os dados do DTO fornecido.
    /// </summary>
    /// <param name="jobOpeningDTO">O DTO da vaga de emprego a ser inserido. Este DTO é mapeado para o modelo de domínio antes da inserção.</param>
    /// <returns>
    /// O identificador da vaga de emprego inserida se a operação for bem-sucedida; caso contrário, retorna 0. Se a validação falhar ou ocorrer uma exceção, também retorna 0.
    /// </returns>
    /// <exception cref="Exception">Captura e notifica qualquer exceção que ocorra durante a operação de inserção, registrando a mensagem de erro apropriada.</exception>
    public async Task<int> InsertAsync(JobOpeningDTO jobOpeningDTO)
    {
        try
        {
            var jobOpening = _imapper.Map<JobOpening>(jobOpeningDTO);
            if (!Validate(jobOpening)) return 0;

            var saved = await _repositoryJobOpening.Insert(jobOpening);
            if (saved == 0)
            {
                Notify(Resource.ErrorMessageSave);
                return 0;
            }

            if (jobOpeningDTO.ListResponsibility.Any())
            {
                foreach (var item in jobOpeningDTO.ListResponsibility)
                {
                    item.JobOpeningId = saved;
                    await _iServiceResponsibility.InsertAsync(item);
                }
            }

            jobOpeningDTO.JobOpeningId = jobOpening.JobOpeningId;
            return jobOpeningDTO.JobOpeningId;
        }
        catch (Exception ex)
        {
            Notify(string.Format(Resource.ErrorMessageException, ex.Message));
            return 0;
        }
    }

    /// <summary>
    /// Atualiza de forma assíncrona uma vaga de emprego existente no repositório após mapear e validar os dados do DTO fornecido.
    /// </summary>
    /// <param name="jobOpeningDTO">O DTO da vaga de emprego a ser atualizado. Este DTO é mapeado para o modelo de domínio antes da atualização.</param>
    /// <returns>
    /// O identificador da vaga de emprego atualizada se a operação for bem-sucedida; caso contrário, retorna 0. Retorna 0 também se a validação falhar ou se ocorrer uma exceção durante a operação.
    /// </returns>
    /// <exception cref="Exception">Captura e notifica qualquer exceção que ocorra durante a operação de atualização, registrando a mensagem de erro apropriada.</exception>
    public async Task<int> UpdateAsync(JobOpeningDTO jobOpeningDTO)
    {
        try
        {
            var jobOpening = _imapper.Map<JobOpening>(jobOpeningDTO);
            if (!Validate(jobOpening)) return 0;

            var saved = await _repositoryJobOpening.Update(jobOpening);
            if (saved == 0)
            {
                Notify(Resource.ErrorMessageUpdate);
                return 0;
            }

            // Processo para inserir e alterar dados das responsabilidade da vaga.
            if (jobOpeningDTO.ListResponsibility.Any())
            {
                await ProcessInsertOrUpdateResponsibility(jobOpeningDTO);
            }

            return jobOpening.JobOpeningId;
        }
        catch (Exception ex)
        {
            Notify(string.Format(Resource.ErrorMessageException, ex.Message));
            return 0;
        }
    }

    /// <summary>
    /// Remove de forma assíncrona uma vaga de emprego existente no repositório com base no ID fornecido.
    /// </summary>
    /// <param name="id">O identificador único da vaga de emprego a ser removida.</param>
    /// <returns>
    /// Retorna 1 se a vaga de emprego for removida com sucesso; caso contrário, retorna 0. Retorna 0 também se a vaga não for encontrada ou se ocorrer uma exceção durante a operação de remoção.
    /// </returns>
    /// <exception cref="Exception">Captura e notifica qualquer exceção que ocorra durante a operação de remoção, registrando a mensagem de erro apropriada.</exception>
    public async Task<int> DeleteAsync(int id)
    {
        try
        {
            var jobOpening = await _repositoryJobOpening.SelectById(id);
            if (jobOpening == null)
            {
                Notify(Resource.RecordNotFoundErrorMessage);
                return 0;
            }

            var saved = await _repositoryJobOpening.Delete(id);

            if (saved == 0)
            {
                Notify(Resource.ErrorMessageDelete);
                return 0;
            }

            var listResponsibilityDelete = await _iServiceResponsibility.SelectAllByJobOpening(id);

            foreach (var item in listResponsibilityDelete)
            {
                await _iServiceResponsibility.DeleteAsync(item.ResponsibilityId);
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
    /// Processa a inserção ou atualização de responsabilidades com base no JobOpeningDTO fornecido.
    /// </summary>
    /// <param name="jobOpeningDTO">O DTO contendo uma lista de responsabilidades para serem inseridas ou atualizadas.</param>
    /// <remarks>
    /// Este método separa as responsabilidades em duas listas: novas responsabilidades (para serem inseridas) e responsabilidades existentes (para serem atualizadas).
    /// - As novas responsabilidades são identificadas por um ResponsibilityId igual a 0, e cada uma é inserida usando o método InsertAsync do serviço _iServiceResponsibility.
    /// - As responsabilidades existentes são identificadas por um ResponsibilityId diferente de 0, e cada uma é atualizada usando o método UpdateAsync do serviço _iServiceResponsibility.
    /// Este método trata essas operações de forma assíncrona, processando cada responsabilidade nas listas de acordo.
    /// </remarks>
    private async Task ProcessInsertOrUpdateResponsibility(JobOpeningDTO jobOpeningDTO)
    {
        List<ResponsibilityDTO> newResponsibilityDTO = new List<ResponsibilityDTO>();
        List<ResponsibilityDTO> updateResponsibilityDTO = new List<ResponsibilityDTO>();

        newResponsibilityDTO = jobOpeningDTO.ListResponsibility.Where(x => x.ResponsibilityId == 0).ToList();
        updateResponsibilityDTO = jobOpeningDTO.ListResponsibility.Where(x => x.ResponsibilityId != 0).ToList();

        if (newResponsibilityDTO.Any())
        {
            foreach (var item in newResponsibilityDTO)
            {
                await _iServiceResponsibility.InsertAsync(item);
            }
        }

        if (updateResponsibilityDTO.Any())
        {
            foreach (var item in updateResponsibilityDTO)
            {
                await _iServiceResponsibility.UpdateAsync(item);
            }
        }
    }

    /// <summary>
    /// Valida uma vaga de emprego com base nos critérios definidos na classe de validação 'ValidationJobOpening'.
    /// </summary>
    /// <param name="jobOpening">O objeto 'JobOpening' a ser validado.</param>
    /// <returns>
    /// Retorna 'true' se a vaga de emprego passar em todos os critérios de validação; caso contrário, retorna 'false'.
    /// </returns>
    private bool Validate(JobOpening jobOpening)
    {
        if (!RunValidation(new ValidationJobOpening(), jobOpening)) return false;

        return true;
    }
}
