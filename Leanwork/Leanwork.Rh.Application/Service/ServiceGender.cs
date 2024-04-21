using AutoMapper;
using Leanwork.Rh.Application.Service;
using Leanwork.Rh.Domain;
using Leanwork.Rh.Domain.Interface;

namespace Leanwork.Rh.Application;

public class ServiceGender : ServiceBase, IServiceGender
{
    IRepositoryGender _repositoryGender;
    private IMapper _mapper;

    public ServiceGender(INotificationError notificationError, IMapper mapper, IRepositoryGender repositoryGender) : base(notificationError)
    {
        _repositoryGender = repositoryGender;
        _mapper = mapper;
    }

    /// <summary>
    /// Exclui um registro de gênero existente no banco de dados de forma assíncrona, utilizando seu identificador.
    /// </summary>
    /// <param name="id">O identificador do gênero a ser excluído.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e retorna um inteiro indicando o sucesso da operação.
    /// Retorna o número de registros afetados pela exclusão, ou zero em caso de falha na operação.
    /// </returns>
    /// <remarks>
    /// Este método prepara e executa uma operação de exclusão no banco de dados utilizando um procedimento armazenado.
    /// O identificador do gênero é utilizado como parâmetro para especificar qual registro deve ser excluído.
    /// A execução do procedimento armazenado é feita de forma assíncrona para evitar bloqueios da thread principal e
    /// melhorar a eficiência da aplicação. Em caso de exceção, como falha na conexão ou erro na execução do SQL,
    /// o método captura a exceção e retorna zero, indicando que a exclusão falhou.
    /// </remarks>
    public async Task<IEnumerable<GenderDTO>> SelectAllAsync()
    {
        var gender = await _repositoryGender.SelectAll();
        return _mapper.Map<IEnumerable<GenderDTO>>(gender);
    }

    /// <summary>
    /// Recupera de forma assíncrona todos os registros de gênero do repositório e os converte para uma lista de objetos DTO.
    /// </summary>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e retorna uma coleção de objetos <see cref="GenderDTO"/> 
    /// contendo os dados dos gêneros. Cada objeto DTO representa um registro de gênero.
    /// </returns>
    /// <remarks>
    /// Este método chama o método 'SelectAll' do repositório '_repositoryGender' para obter todos os registros de gêneros disponíveis.
    /// Após a recuperação dos dados, utiliza o objeto '_mapper' para converter a lista de entidades 'Gender' em uma lista de
    /// objetos 'GenderDTO', que são mais adequados para transferência de dados em operações de front-end ou APIs, pois podem
    /// conter apenas os dados necessários ou estruturados de forma diferente em comparação com as entidades de domínio.
    /// </remarks>
    public async Task<GenderDTO> SelectByIdAsync(int id)
    {
        var gender = await _repositoryGender.SelectById(id);
        return _mapper.Map<GenderDTO>(gender);
    }

    /// <summary>
    /// Insere um novo registro de gênero no banco de dados de forma assíncrona a partir de um objeto DTO.
    /// </summary>
    /// <param name="genderDTO">O objeto DTO <see cref="GenderDTO"/> que contém os dados do gênero a ser inserido.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e retorna o identificador do gênero inserido,
    /// ou zero em caso de falha na inserção.
    /// </returns>
    /// <remarks>
    /// Este método converte inicialmente o objeto DTO para um objeto de domínio 'Gender' usando o mapeador.
    /// Após a conversão, o gênero é validado usando o método 'Validate'. Se a validação falhar, o método retorna zero.
    /// Se a validação for bem-sucedida, o método tenta inserir o gênero no banco de dados através do repositório 'repositoryGender'.
    /// Se a inserção falhar (por exemplo, por violação de restrições do banco de dados), uma mensagem de erro é notificada,
    /// e o método também retorna zero. Em caso de exceção durante o processo, uma mensagem de erro é notificada, e o método retorna zero.
    /// Se a inserção for bem-sucedida, o identificador do gênero é atualizado no DTO e retornado.
    /// </remarks>
    public async Task<int> InsertAsync(GenderDTO genderDTO)
    {
        try
        {
            var gender = _mapper.Map<Gender>(genderDTO);
            if (!Validate(gender)) return 0;

            var saved = await _repositoryGender.Insert(gender);
            if (saved == 0)
            {
                Notify(Resource.ErrorMessageSave);
                return 0;
            }

            genderDTO.GenderId = gender.GenderId;
            return genderDTO.GenderId;
        }
        catch (Exception ex)
        {
            Notify(string.Format(Resource.ErrorMessageException, ex.Message));
            return 0;
        }
    }

    /// <summary>
    /// Atualiza os dados de um registro de gênero existente no banco de dados de forma assíncrona a partir de um objeto DTO.
    /// </summary>
    /// <param name="genderDTO">O objeto DTO <see cref="GenderDTO"/> que contém os dados atualizados do gênero.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e retorna o identificador do gênero atualizado,
    /// ou zero em caso de falha na atualização.
    /// </returns>
    /// <remarks>
    /// Este método inicia convertendo o objeto DTO para um objeto de domínio 'Gender' usando o mapeador.
    /// Após a conversão, o gênero é validado usando o método 'Validate'. Se a validação falhar, o método retorna zero.
    /// Se a validação for bem-sucedida, o método tenta atualizar o gênero no banco de dados através do repositório 'repositoryGender'.
    /// Se a atualização falhar (por exemplo, se nenhum registro for alterado devido à inexistência do gênero),
    /// uma mensagem de erro é notificada, e o método também retorna zero. Em caso de exceção durante o processo,
    /// uma mensagem de erro é notificada, e o método retorna zero.
    /// Se a atualização for bem-sucedida, o identificador do gênero é retornado.
    /// </remarks>
    public async Task<int> UpdateAsync(GenderDTO genderDTO)
    {
        try
        {
            var gender = _mapper.Map<Gender>(genderDTO);
            if (!Validate(gender)) return 0;

            var saved = await _repositoryGender.Update(gender);
            if (saved == 0)
            {
                Notify(Resource.ErrorMessageUpdate);
                return 0;
            }

            return gender.GenderId;
        }
        catch (Exception ex)
        {
            Notify(string.Format(Resource.ErrorMessageException, ex.Message));
            return 0;
        }
    }

    /// <summary>
    /// Remove um registro de gênero do banco de dados de forma assíncrona utilizando seu identificador.
    /// </summary>
    /// <param name="id">O identificador do gênero a ser excluído.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e retorna um inteiro indicando o resultado da operação.
    /// Retorna 1 se a exclusão for bem-sucedida, ou zero se a exclusão falhar ou ocorrer uma exceção.
    /// </returns>
    /// <remarks>
    /// Este método inicia verificando a existência do gênero no banco de dados chamando o método 'SelectById' do repositório '_repositoryGender'.
    /// Se nenhum gênero for encontrado com o identificador fornecido, uma mensagem de erro é notificada e o método retorna zero.
    /// Se um gênero é encontrado, ele tenta excluí-lo do banco de dados. Se a operação de exclusão falhar (nenhum registro for afetado),
    /// uma mensagem de erro é notificada, e o método também retorna zero. Em caso de exceção durante o processo, uma mensagem de erro é
    /// notificada, e o método retorna zero. Se a exclusão for bem-sucedida, o método retorna 1.
    /// </remarks>
    public async Task<int> DeleteAsync(int id)
    {
        try
        {
            var gender = await _repositoryGender.SelectById(id);
            if (gender == null)
            {
                Notify(Resource.RecordNotFoundErrorMessage);
                return 0;
            }

            var saved = await _repositoryGender.Delete(id);

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
    /// Valida um objeto <see cref="Gender"/> usando uma classe de validação específica.
    /// </summary>
    /// <param name="gender">O objeto <see cref="Gender"/> a ser validado.</param>
    /// <returns>
    /// Retorna <c>true</c> se o gênero passar em todas as validações; caso contrário, retorna <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método utiliza a classe 'ValidationGender' para executar as validações necessárias no objeto gênero.
    /// A classe 'ValidationGender' contém regras específicas que devem ser cumpridas para que o objeto seja considerado válido.
    /// Se qualquer uma dessas validações falhar, o método retorna <c>false</c>, indicando que o gênero não atende a todos os
    /// critérios de validação estabelecidos.
    /// </remarks>
    private bool Validate(Gender gender)
    {
        if (!RunValidation(new ValidationGender(), gender)) return false;

        return true;
    }
}
