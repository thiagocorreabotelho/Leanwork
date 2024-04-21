using AutoMapper;
using Leanwork.Rh.Application.Service;
using Leanwork.Rh.Domain;
using Leanwork.Rh.Domain.Interface;

namespace Leanwork.Rh.Application;

public class ServiceAddress : ServiceBase, IServiceAddress
{
    private IRepositoryAddress _repositoryAddress;
    private IMapper _mapper;

    public ServiceAddress(INotificationError notificationError, IMapper mapper, IRepositoryAddress repositoryAddress) : base(notificationError)
    {
        _repositoryAddress = repositoryAddress;
        _mapper = mapper;
    }

    /// <summary>
    /// Recupera de forma assíncrona todos os endereços associados a uma empresa específica do banco de dados e os converte para uma lista de objetos DTO.
    /// </summary>
    /// <param name="id">O identificador da empresa cujos endereços serão recuperados.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e retorna uma coleção de objetos <see cref="AddressDTO"/>
    /// contendo os dados dos endereços da empresa especificada.
    /// </returns>
    /// <remarks>
    /// Este método chama o método 'SelectAllByCompany' do repositório '_repositoryAddress' para obter todos os endereços vinculados a uma empresa específica.
    /// Após a recuperação dos dados, utiliza o objeto '_mapper' para converter a lista de entidades 'Address' em uma lista de
    /// objetos 'AddressDTO', que são mais adequados para transferência de dados em operações de front-end ou APIs, pois podem
    /// conter apenas os dados necessários ou estruturados de forma diferente em comparação com as entidades de domínio.
    /// Este método é útil para obter rapidamente uma visão completa dos endereços associados a uma empresa, o que pode ser 
    /// essencial para processos de verificação de informações ou logística relacionada à empresa.
    /// </remarks>
    public async Task<IEnumerable<AddressDTO>> SelectAllByCompanyAsync(int id)
    {
        var address = await _repositoryAddress.SelectAllByCompany(id);
        return _mapper.Map<IEnumerable<AddressDTO>>(address);
    }

    /// <summary>
    /// Recupera de forma assíncrona todos os endereços associados a um candidato específico do banco de dados e os converte para uma lista de objetos DTO.
    /// </summary>
    /// <param name="id">O identificador do candidato cujos endereços serão recuperados.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e retorna uma coleção de objetos <see cref="AddressDTO"/>
    /// contendo os dados dos endereços do candidato especificado.
    /// </returns>
    /// <remarks>
    /// Este método chama o método 'SelectAllByCandidate' do repositório '_repositoryAddress' para obter todos os endereços vinculados a um candidato específico.
    /// Após a recuperação dos dados, utiliza o objeto '_mapper' para converter a lista de entidades 'Address' em uma lista de
    /// objetos 'AddressDTO', que são mais adequados para transferência de dados em operações de front-end ou APIs. Esta conversão é crucial para assegurar que
    /// os dados transmitidos sejam adequados para consumo externo, contendo apenas os detalhes necessários e formatados de maneira ideal para os consumidores da API.
    /// Este método é especialmente útil para operações logísticas, verificações de informações ou serviços que necessitem de um entendimento completo dos locais associados a um candidato.
    /// </remarks>
    public async Task<IEnumerable<AddressDTO>> SelectAllByCandidateAsync(int id)
    {
        var address = await _repositoryAddress.SelectAllByCandidate(id);
        return _mapper.Map<IEnumerable<AddressDTO>>(address);
    }

    /// <summary>
    /// Recupera de forma assíncrona um endereço específico do banco de dados pelo seu identificador e o converte para um objeto DTO.
    /// </summary>
    /// <param name="id">O identificador do endereço a ser recuperado.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona, retornando o objeto <see cref="AddressDTO"/> correspondente ao identificador fornecido,
    /// ou <c>null</c> se nenhum endereço for encontrado.
    /// </returns>
    /// <remarks>
    /// Este método chama o método 'SelectById' do repositório '_repositoryAddress' para obter um endereço específico.
    /// Após a recuperação dos dados, utiliza o objeto '_mapper' para converter a entidade 'Address' em um objeto 'AddressDTO'.
    /// Os objetos DTO são utilizados para transferir dados de uma forma que seja mais conveniente para operações de front-end ou APIs,
    /// encapsulando os dados necessários de maneira otimizada e segura.
    /// Este método é essencial para operações que requerem detalhamento de informações de um endereço específico para ações como verificação,
    /// atualizações ou exibições detalhadas em interfaces de usuário.
    /// </remarks>
    public async Task<AddressDTO> SelectByIdAsync(int id)
    {
        var address = await _repositoryAddress.SelectById(id);
        return _mapper.Map<AddressDTO>(address);
    }

    /// <summary>
    /// Insere um novo endereço no banco de dados de forma assíncrona a partir de um objeto DTO.
    /// </summary>
    /// <param name="addressDTO">O objeto DTO <see cref="AddressDTO"/> que contém os dados do endereço a ser inserido.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e retorna o identificador do endereço inserido,
    /// ou zero em caso de falha na inserção.
    /// </returns>
    /// <remarks>
    /// Este método converte inicialmente o objeto DTO para um objeto de domínio 'Address' usando o mapeador.
    /// Após a conversão, o endereço é validado usando o método 'Validate'. Se a validação falhar, o método retorna zero.
    /// Se a validação for bem-sucedida, o método tenta inserir o endereço no banco de dados através do repositório 'repositoryAddress'.
    /// Se a inserção falhar (por exemplo, por violação de restrições do banco de dados), uma mensagem de erro é notificada,
    /// e o método também retorna zero. Em caso de exceção durante o processo, uma mensagem de erro é notificada, e o método retorna zero.
    /// Se a inserção for bem-sucedida, o identificador do endereço é atualizado no DTO e retornado.
    /// </remarks>
    public async Task<int> InsertAsync(AddressDTO addressDTO)
    {
        try
        {
            var address = _mapper.Map<Address>(addressDTO);
            if (!Validate(address)) return 0;

            var saved = await _repositoryAddress.Insert(address);
            if (saved == 0)
            {
                Notify(Resource.ErrorMessageSave);
                return 0;
            }

            addressDTO.AddressId = address.AddressId;
            return addressDTO.AddressId;
        }
        catch (Exception ex)
        {
            Notify(string.Format(Resource.ErrorMessageException, ex.Message));
            return 0;
        }
    }

    /// <summary>
    /// Atualiza os dados de um endereço existente no banco de dados de forma assíncrona a partir de um objeto DTO.
    /// </summary>
    /// <param name="addressDTO">O objeto DTO <see cref="AddressDTO"/> que contém os dados atualizados do endereço.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e retorna o identificador do endereço atualizado,
    /// ou zero em caso de falha na atualização.
    /// </returns>
    /// <remarks>
    /// Este método inicia convertendo o objeto DTO para um objeto de domínio 'Address' usando o mapeador.
    /// Após a conversão, o endereço é validado usando o método 'Validate'. Se a validação falhar, o método retorna zero.
    /// Se a validação for bem-sucedida, o método tenta atualizar o endereço no banco de dados através do repositório 'repositoryAddress'.
    /// Se a atualização falhar (por exemplo, se nenhum registro for alterado devido à inexistência do endereço),
    /// uma mensagem de erro é notificada, e o método também retorna zero. Em caso de exceção durante o processo,
    /// uma mensagem de erro é notificada, e o método retorna zero.
    /// Se a atualização for bem-sucedida, o identificador do endereço é retornado.
    /// </remarks>
    public async Task<int> UpdateAsync(AddressDTO addressDTO)
    {
        try
        {
            var address = _mapper.Map<Address>(addressDTO);
            if (!Validate(address)) return 0;

            var saved = await _repositoryAddress.Update(address);
            if (saved == 0)
            {
                Notify(Resource.ErrorMessageUpdate);
                return 0;
            }

            return address.AddressId;
        }
        catch (Exception ex)
        {
            Notify(string.Format(Resource.ErrorMessageException, ex.Message));
            return 0;
        }
    }

    /// <summary>
    /// Exclui um endereço do banco de dados de forma assíncrona utilizando seu identificador.
    /// </summary>
    /// <param name="id">O identificador do endereço a ser excluído.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e retorna um inteiro indicando o resultado da operação.
    /// Retorna 1 se a exclusão for bem-sucedida, ou zero se a exclusão falhar ou ocorrer uma exceção.
    /// </returns>
    /// <remarks>
    /// Este método inicia verificando a existência do endereço no banco de dados chamando o método 'SelectById' do repositório '_repositoryAddress'.
    /// Se nenhum endereço for encontrado com o identificador fornecido, uma mensagem de erro é notificada e o método retorna zero.
    /// Se um endereço é encontrado, ele tenta excluí-lo do banco de dados. Se a operação de exclusão falhar (nenhum registro for afetado),
    /// uma mensagem de erro é notificada, e o método também retorna zero. Em caso de exceção durante o processo, uma mensagem de erro é
    /// notificada, e o método retorna zero. Se a exclusão for bem-sucedida, o método retorna 1.
    /// </remarks>

    public async Task<int> DeleteAsync(int id)
    {
        try
        {
            var address = await _repositoryAddress.SelectById(id);
            if (address == null)
            {
                Notify(Resource.RecordNotFoundErrorMessage);
                return 0;
            }

            var saved = await _repositoryAddress.Delete(id);

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
    /// Exclui todos os endereços associados a um candidato específico de forma assíncrona.
    /// </summary>
    /// <param name="id">O identificador do candidato cujos endereços serão excluídos.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e retorna um inteiro indicando o sucesso ou fracasso da exclusão.
    /// Retorna 1 se a exclusão for bem-sucedida, ou zero em caso de falha.
    /// </returns>
    /// <remarks>
    /// Este método invoca o método 'DeleteAllCandidateAsync' do repositório '_repositoryAddress' para excluir todos os endereços
    /// vinculados ao identificador de um candidato fornecido. Se a operação no repositório não afetar nenhum registro (retorna 0),
    /// uma notificação de erro é emitida usando o recurso 'Resource.ErrorMessageDelete' e o método retorna zero, indicando falha na exclusão.
    /// Em caso de sucesso, o método retorna 1. Se uma exceção é capturada durante o processo, uma mensagem de erro é notificada,
    /// utilizando 'Resource.ErrorMessageException', e o método também retorna zero.
    /// Este método é útil para manter a integridade dos dados quando um candidato é removido do sistema, assegurando que todos os endereços
    /// associados sejam também removidos.
    /// </remarks>
    public async Task<int> DeleteAllCandidateAsync(int id)
    {
        try
        {
            var saved = await _repositoryAddress.DeleteAllCandidateAsync(id);

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
    /// Exclui todos os endereços associados a uma empresa específica de forma assíncrona.
    /// </summary>
    /// <param name="id">O identificador da empresa cujos endereços serão excluídos.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e retorna um inteiro indicando o sucesso ou fracasso da exclusão.
    /// Retorna 1 se a exclusão for bem-sucedida, ou zero em caso de falha.
    /// </returns>
    /// <remarks>
    /// Este método invoca o método 'DeleteAllCompanyAsync' do repositório '_repositoryAddress' para excluir todos os endereços
    /// vinculados ao identificador de uma empresa fornecida. Se a operação no repositório não afetar nenhum registro (retorna 0),
    /// uma notificação de erro é emitida usando o recurso 'Resource.ErrorMessageDelete' e o método retorna zero, indicando falha na exclusão.
    /// Em caso de sucesso, o método retorna 1. Se uma exceção é capturada durante o processo, uma mensagem de erro é notificada,
    /// utilizando 'Resource.ErrorMessageException', e o método também retorna zero.
    /// Este método é útil para manter a integridade dos dados quando uma empresa é removida do sistema, assegurando que todos os endereços
    /// associados sejam também removidos.
    /// </remarks>
    public async Task<int> DeleteAllCompanyAsync(int id)
    {
        try
        {
            var saved = await _repositoryAddress.DeleteAllCompanyAsync(id);

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
    /// Valida um objeto <see cref="Address"/> usando uma classe de validação específica para garantir que todos os campos obrigatórios estejam corretos.
    /// </summary>
    /// <param name="address">O objeto <see cref="Address"/> a ser validado.</param>
    /// <returns>
    /// Retorna <c>true</c> se o endereço passar em todas as validações; caso contrário, retorna <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método utiliza a classe 'ValidationAddress' para executar as validações necessárias no objeto endereço.
    /// A classe 'ValidationAddress' contém regras específicas que devem ser cumpridas para que o objeto seja considerado válido.
    /// Se qualquer uma dessas validações falhar, o método retorna <c>false</c>, indicando que o endereço não atende a todos os
    /// critérios de validação estabelecidos.
    /// Este método é utilizado principalmente antes de operações de inserção ou atualização para assegurar a integridade dos dados.
    /// </remarks>
    private bool Validate(Address address)
    {
        if (!RunValidation(new ValidationAddress(), address)) return false;

        return true;
    }

}
