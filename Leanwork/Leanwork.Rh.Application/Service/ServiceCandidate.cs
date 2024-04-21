using AutoMapper;
using Leanwork.Rh.Application.DTO.Technology;
using Leanwork.Rh.Application.Interface;
using Leanwork.Rh.Application.Service;
using Leanwork.Rh.Domain;
using Leanwork.Rh.Domain.Interface;

namespace Leanwork.Rh.Application;

public class ServiceCandidate : ServiceBase, IServiceCandidate
{
    private IRepositoryCandidate _repositoryCandidate;
    private IServiceAddress _serviceAddress;
    private IServiceTechnology _serviceTechnology;
    private IServiceCandidateTechnologyRel _serviceCandidateTechnologyRel;
    private IMapper _mapper;

    public ServiceCandidate(INotificationError notificationError, IMapper mapper, IRepositoryCandidate repositoryCandidate, IServiceAddress serviceAddress, IServiceTechnology serviceTechnology, IServiceCandidateTechnologyRel serviceCandidateTechnologyRel) : base(notificationError)
    {
        _serviceAddress = serviceAddress;
        _repositoryCandidate = repositoryCandidate;
        _serviceTechnology = serviceTechnology;
        _serviceCandidateTechnologyRel = serviceCandidateTechnologyRel;
        _mapper = mapper;
    }

    /// <summary>
    /// Recupera de forma assíncrona todos os candidatos do repositório e os converte para uma lista de objetos DTO.
    /// </summary>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e retorna uma coleção de objetos <see cref="CandidateDTO"/>
    /// contendo os dados dos candidatos. Cada objeto DTO representa um candidato.
    /// </returns>
    /// <remarks>
    /// Este método chama o método 'SelectAll' do repositório '_repositoryCandidate' para obter todos os candidatos existentes.
    /// Após a recuperação dos dados, utiliza o objeto '_mapper' para converter a lista de entidades 'Candidate' em uma lista de
    /// objetos 'CandidateDTO', que são mais adequados para transferência de dados em operações de front-end ou APIs, pois podem
    /// conter apenas os dados necessários ou estruturados de forma diferente em comparação com as entidades de domínio.
    /// </remarks>
    public async Task<IEnumerable<CandidateDTO>> SelectAllAsync()
    {
        var candidate = await _repositoryCandidate.SelectAll();
        return _mapper.Map<IEnumerable<CandidateDTO>>(candidate);
    }

    /// <summary>
    /// Recupera de forma assíncrona um candidato específico do repositório usando seu identificador e o converte para um objeto DTO.
    /// </summary>
    /// <param name="id">O identificador do candidato a ser recuperado.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e retorna um objeto <see cref="CandidateDTO"/>
    /// contendo os dados do candidato. Retorna null se nenhum candidato for encontrado com o identificador fornecido.
    /// </returns>
    /// <remarks>
    /// Este método primeiro chama o método 'SelectById' do repositório '_repositoryCandidate' para obter um candidato
    /// pelo seu identificador. Após obter o candidato, se um registro é encontrado, ele é convertido de uma entidade de 
    /// domínio para um objeto DTO utilizando o objeto '_mapper'. Os objetos DTO são úteis para encapsular dados de forma 
    /// a adaptá-los para transferências específicas, como em respostas de APIs, minimizando a exposição de dados internos
    /// e melhorando a segurança e a manutenibilidade do código.
    /// </remarks>
    public async Task<CandidateDTO> SelectByIdAsync(int id)
    {
        var candidate = await _repositoryCandidate.SelectById(id);
        var candidateDTO = _mapper.Map<CandidateDTO>(candidate);
        
        var addressDTO = await _serviceAddress.SelectAllByCandidateAsync(candidateDTO.CandidateId);
        candidateDTO.ListAddress = addressDTO.ToList();

        var technologies = await _serviceCandidateTechnologyRel.SelectAllTechnologiesByCandidateAsync(id);
        var convertListTechnologies =  _mapper.Map<IEnumerable<TechnologyDTO>>(technologies);   
        candidateDTO.ListTechnologies = convertListTechnologies.ToList();

        return candidateDTO;
    }

    /// <summary>
    /// Insere um novo candidato no banco de dados de forma assíncrona a partir de um objeto DTO.
    /// </summary>
    /// <param name="candidateDTO">O objeto DTO <see cref="CandidateDTO"/> que contém os dados do candidato a ser inserido.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e retorna o identificador do candidato inserido,
    /// ou zero em caso de falha na inserção.
    /// </returns>
    /// <remarks>
    /// Este método converte inicialmente o objeto DTO para um objeto de domínio 'Candidate' usando o mapeador.
    /// Após a conversão, o candidato é validado usando o método 'Validate'. Se a validação falhar, o método retorna zero.
    /// Se a validação for bem-sucedida, o método tenta inserir o candidato no banco de dados através do repositório 'repositoryCandidate'.
    /// Se a inserção falhar (por exemplo, por violação de restrições do banco de dados), uma mensagem de erro é notificada,
    /// e o método também retorna zero. Em caso de exceção durante o processo, uma mensagem de erro é notificada, e o método retorna zero.
    /// Se a inserção for bem-sucedida, o identificador do candidato é atualizado no DTO e retornado.
    /// </remarks>
    public async Task<int> InsertAsync(CandidateDTO candidateDTO)
    {
        try
        {
            var candidate = _mapper.Map<Candidate>(candidateDTO);
            if (!Validate(candidate)) return 0;

            var saved = await _repositoryCandidate.Insert(candidate);
            if (saved == 0)
            {
                Notify(Resource.ErrorMessageSave);
                return 0;
            }

            if (candidateDTO.ListAddress.Any())
            {
                foreach (var item in candidateDTO.ListAddress)
                {
                    item.CandidateId = saved;
                    var addressDTO = _mapper.Map<AddressDTO>(item);
                    await _serviceAddress.InsertAsync(addressDTO);
                }
            }

            candidateDTO.CandidateId = candidate.CandidateId;
            return candidateDTO.CandidateId;
        }
        catch (Exception ex)
        {
            Notify(string.Format(Resource.ErrorMessageException, ex.Message));
            return 0;
        }
    }

    /// <summary>
    /// Atualiza os dados de um candidato existente no banco de dados de forma assíncrona a partir de um objeto DTO.
    /// </summary>
    /// <param name="candidateDTO">O objeto DTO <see cref="CandidateDTO"/> que contém os dados atualizados do candidato.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e retorna o identificador do candidato atualizado,
    /// ou zero em caso de falha na atualização.
    /// </returns>
    /// <remarks>
    /// Este método começa convertendo o objeto DTO para um objeto de domínio 'Candidate' usando o mapeador.
    /// Após a conversão, o candidato é validado usando o método 'Validate'. Se a validação falhar, o método retorna zero.
    /// Se a validação for bem-sucedida, o método tenta atualizar o candidato no banco de dados através do repositório 'repositoryCandidate'.
    /// Se a atualização falhar (por exemplo, se nenhum registro for alterado devido à inexistência do candidato),
    /// uma mensagem de erro é notificada, e o método também retorna zero. Em caso de exceção durante o processo,
    /// uma mensagem de erro é notificada, e o método retorna zero.
    /// Se a atualização for bem-sucedida, o identificador do candidato é retornado.
    /// </remarks>
    public async Task<int> UpdateAsync(CandidateDTO candidateDTO)
    {
        try
        {
            var candidate = _mapper.Map<Candidate>(candidateDTO);
            if (!Validate(candidate)) return 0;

            var saved = await _repositoryCandidate.Update(candidate);
            if (saved == 0)
            {
                Notify(Resource.ErrorMessageUpdate);
                return 0;
            }

            if (candidateDTO.ListAddress.Any())
            {
                await ProcessInsertOrUpdateAddress(candidateDTO);
            }

            return candidate.CandidateId;
        }
        catch (Exception ex)
        {
            Notify(string.Format(Resource.ErrorMessageException, ex.Message));
            return 0;
        }
    }

    /// <summary>
    /// Remove um candidato existente no banco de dados de forma assíncrona usando seu identificador.
    /// </summary>
    /// <param name="id">O identificador do candidato a ser removido.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e retorna um inteiro indicando o resultado da operação.
    /// Retorna 1 se a exclusão for bem-sucedida, ou zero se a exclusão falhar ou ocorrer uma exceção.
    /// </returns>
    /// <remarks>
    /// Este método inicia verificando a existência do candidato no banco de dados chamando 'SelectById' do repositório '_repositoryCandidate'.
    /// Se nenhum candidato for encontrado com o identificador fornecido, uma mensagem de erro é notificada e o método retorna zero.
    /// Se um candidato é encontrado, ele tenta excluí-lo do banco de dados. Se a operação de exclusão falhar (nenhum registro for afetado),
    /// uma mensagem de erro é notificada, e o método também retorna zero. Em caso de exceção durante o processo, uma mensagem de erro é
    /// notificada, e o método retorna zero. Se a exclusão for bem-sucedida, o método retorna 1.
    /// </remarks>
    public async Task<int> DeleteAsync(int id)
    {
        try
        {
            var candidate = await _repositoryCandidate.SelectById(id);
            if (candidate == null)
            {
                Notify(Resource.RecordNotFoundErrorMessage);
                return 0;
            }

            var saved = await _repositoryCandidate.Delete(id);

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
    /// Processa a inserção ou atualização de endereços para um candidato dado.
    /// </summary>
    /// <param name="candidateDTO">O DTO do candidato que contém a lista de endereços a serem processados.</param>
    /// <remarks>
    /// Este método separa os endereços novos dos existentes com base no ID do endereço.
    /// Se o ID do endereço é zero, o endereço é considerado novo e será inserido.
    /// Se o ID do endereço é diferente de zero, o endereço é considerado existente e será atualizado.
    /// </remarks>
    private async Task ProcessInsertOrUpdateAddress(CandidateDTO candidateDTO)
    {
        List<AddressDTO> newAddressDTO = new List<AddressDTO>();
        List<AddressDTO> updateAddressDTO = new List<AddressDTO>();

        newAddressDTO = candidateDTO.ListAddress.Where(x => x.AddressId == 0).ToList();
        updateAddressDTO = candidateDTO.ListAddress.Where(x => x.AddressId != 0).ToList();

        if (newAddressDTO.Any())
        {
            foreach (var item in newAddressDTO)
            {
                await _serviceAddress.InsertAsync(item);
            }
        }

        if (updateAddressDTO.Any())
        {
            foreach (var item in updateAddressDTO)
            {
                await _serviceAddress.UpdateAsync(item);
            }
        }
    }


    /// <summary>
    /// Valida um objeto <see cref="Candidate"/> usando uma classe de validação específica.
    /// </summary>
    /// <param name="candidate">O objeto <see cref="Candidate"/> a ser validado.</param>
    /// <returns>
    /// Retorna <c>true</c> se o candidato passar em todas as validações; caso contrário, retorna <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método utiliza a classe 'ValidationCandidate' para executar as validações necessárias no objeto candidato.
    /// A classe 'ValidationCandidate' deve conter as regras específicas de validação para os atributos do objeto
    /// 'Candidate'. Se qualquer uma dessas validações falhar, o método retorna <c>false>, indicando que o candidato
    /// não é válido de acordo com as regras estabelecidas. Caso contrário, retorna <c>true>, indicando que o candidato
    /// atende a todos os critérios de validação.
    /// </remarks>
    private bool Validate(Candidate candidate)
    {
        if (!RunValidation(new ValidationCandidate(), candidate)) return false;

        return true;
    }
}
