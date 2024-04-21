using AutoMapper;
using Leanwork.Rh.Application.DTO.Technology;
using Leanwork.Rh.Application.Service;
using Leanwork.Rh.Domain;
using Leanwork.Rh.Domain.Interface;

namespace Leanwork.Rh.Application;

public class ServiceCompany : ServiceBase, IServiceCompany
{
    private IServiceCompanyTechnologyRel _iServiceCompanyTechnologyRel;
    private IRepositoryCompany _repositoryCompany;
    private IServiceAddress _serviceAddress;
    private IMapper _mapper;

    public ServiceCompany(INotificationError notificationError, IMapper mapper, IRepositoryCompany repositoryCompany, IServiceAddress serviceAddress, IServiceCompanyTechnologyRel iServiceCompanyTechnologyRel) : base(notificationError)
    {
        _iServiceCompanyTechnologyRel = iServiceCompanyTechnologyRel;
        _serviceAddress = serviceAddress;
        _repositoryCompany = repositoryCompany;
        _mapper = mapper;
    }

    /// <summary>
    /// Recupera todos os registros de empresas do banco de dados e os converte para uma lista de DTOs.
    /// </summary>
    /// <remarks>
    /// Este método consulta o repositório de empresas para obter todos os registros existentes. 
    /// Após a recuperação dos dados, eles são mapeados de entidades `Company` para `CompanyDTO` usando um objeto de mapeamento configurado (geralmente com AutoMapper).
    /// Este processo facilita a transferência de dados desacoplada da estrutura do banco de dados, ideal para operações de API e reduzindo a dependência entre a camada de acesso a dados e a camada de apresentação.
    /// </remarks>
    /// <returns>
    /// Uma tarefa que, quando concluída com sucesso, retorna uma coleção enumerável de `CompanyDTO`, representando todas as empresas. 
    /// A coleção pode estar vazia se não houver registros de empresas no banco de dados.
    /// </returns>
    /// <exception cref="Exception">
    /// Uma exceção pode ser lançada em caso de falha na operação de consulta ou mapeamento. Detalhes da exceção devem ser registrados e podem incluir erros de conexão ao banco de dados, problemas na execução da consulta, ou falhas no mapeamento de dados.
    /// </exception>
    public async Task<IEnumerable<CompanyDTO>> SelectAllAsync()
    {
        var company = await _repositoryCompany.SelectAll();
        return _mapper.Map<IEnumerable<CompanyDTO>>(company);
    }


    /// <summary>
    /// Recupera uma empresa específica pelo seu ID e a converte para um DTO.
    /// </summary>
    /// <remarks>
    /// Este método consulta o repositório de empresas para obter um registro específico baseado no ID fornecido. 
    /// Se a empresa for encontrada, ela é mapeada de uma entidade `Company` para `CompanyDTO` usando um objeto de mapeamento configurado (geralmente com AutoMapper).
    /// Este processo é ideal para APIs, pois facilita a transferência de dados de forma desacoplada da estrutura do banco de dados, 
    /// e melhora a segurança dos dados ao expor apenas os campos necessários na camada de apresentação.
    /// </remarks>
    /// <param name="id">O ID da empresa a ser recuperada.</param>
    /// <returns>
    /// Uma tarefa que, quando concluída com sucesso, retorna o `CompanyDTO` correspondente à empresa encontrada. 
    /// Retorna null se nenhuma empresa for encontrada com o ID fornecido.
    /// </returns>
    /// <exception cref="Exception">
    /// Uma exceção pode ser lançada em caso de falha na operação de consulta ou mapeamento. Detalhes da exceção devem ser registrados e podem incluir erros de conexão ao banco de dados, problemas na execução da consulta, ou falhas no mapeamento de dados.
    /// </exception>
    public async Task<CompanyDTO> SelectByIdAsync(int id)
    {
        var company = await _repositoryCompany.SelectById(id);

        var companyDTO = _mapper.Map<CompanyDTO>(company);
        var addressDTO = await _serviceAddress.SelectAllByCompanyAsync(companyDTO.CompanyId);
        companyDTO.ListAddress = addressDTO.ToList();

        var technologies = await _iServiceCompanyTechnologyRel.SelectAllTechnologiesByCompanyAsync(id);
        var convertListTechnologies =  _mapper.Map<IEnumerable<TechnologyDTO>>(technologies);  
        companyDTO.ListTechnologies  = convertListTechnologies.ToList();
        
        return companyDTO;
    }


    /// <summary>
    /// Insere uma nova empresa no banco de dados após mapeá-la de um DTO.
    /// </summary>
    /// <remarks>
    /// Este método converte um `CompanyDTO` para a entidade `Company` usando AutoMapper. 
    /// Antes da inserção, valida a empresa usando o método `Validate`. Se a validação falhar, 
    /// ou se a inserção não afetar nenhum registro (indicando falha na inserção), o método 
    /// retorna 0 e notifica sobre o erro usando um sistema de notificação.
    /// Se a inserção for bem-sucedida, o ID da empresa é atualizado no DTO e retornado.
    /// </remarks>
    /// <param name="companyDTO">O DTO da empresa que será inserido no banco de dados.</param>
    /// <returns>
    /// O ID da empresa inserida se bem-sucedido; caso contrário, 0.
    /// </returns>
    /// <exception cref="Exception">
    /// Captura e trata qualquer exceção que possa ocorrer durante o processo de mapeamento, validação, 
    /// ou inserção, notificando o erro e retornando 0.
    /// </exception>
    public async Task<int> InsertAsync(CompanyDTO companyDTO)
    {
        try
        {
            var company = _mapper.Map<Company>(companyDTO);
            if (!Validate(company)) return 0;

            var saved = await _repositoryCompany.Insert(company);
            if (saved == 0)
            {
                Notify(Resource.ErrorMessageSave);
                return 0;
            }

            // Processo para inserir novos endereços.
            if (companyDTO.ListAddress.Any())
            {
                foreach (var item in companyDTO.ListAddress)
                {
                    item.CompanyId = saved;
                    var addressDto = _mapper.Map<AddressDTO>(item);
                    await _serviceAddress.InsertAsync(addressDto);
                }
            }

            companyDTO.CompanyId = company.CompanyId;
            return companyDTO.CompanyId;
        }
        catch (Exception ex)
        {
            Notify(string.Format(Resource.ErrorMessageException, ex.Message));
            return 0;
        }
    }

    /// <summary>
    /// Atualiza os dados de uma empresa existente no banco de dados a partir de um DTO fornecido.
    /// </summary>
    /// <remarks>
    /// Este método converte um `CompanyDTO` para a entidade `Company` usando AutoMapper e então valida a empresa usando um método `Validate`.
    /// Se a validação falhar, ou se a atualização não modificar nenhum registro (indicando que a empresa talvez não exista ou os dados não foram alterados),
    /// o método retorna 0 e notifica o erro. Se a atualização for bem-sucedida, retorna o ID da empresa atualizada.
    /// </remarks>
    /// <param name="companyDTO">O DTO da empresa contendo os dados atualizados para a atualização no banco de dados.</param>
    /// <returns>
    /// O ID da empresa atualizada se a operação for bem-sucedida; 0 se a operação falhar ou a validação não for passada.
    /// </returns>
    /// <exception cref="Exception">
    /// Captura e trata qualquer exceção que possa ocorrer durante o processo de mapeamento, validação ou atualização,
    /// notificando o erro usando uma mensagem formatada e retornando 0.
    /// </exception>
    public async Task<int> UpdateAsync(CompanyDTO companyDTO)
    {
        try
        {
            var company = _mapper.Map<Company>(companyDTO);
            if (!Validate(company)) return 0;

            var saved = await _repositoryCompany.Update(company);
            if (saved == 0)
            {
                Notify(Resource.ErrorMessageUpdate);
                return 0;
            }

            if (companyDTO.ListAddress.Any())
            {
                await ProcessInsertOrUpdateAddress(companyDTO);
            }

            return company.CompanyId;
        }
        catch (Exception ex)
        {
            Notify(string.Format(Resource.ErrorMessageException, ex.Message));
            return 0;
        }
    }

    /// <summary>
    /// Exclui um registro de empresa do banco de dados com base no ID fornecido.
    /// </summary>
    /// <remarks>
    /// Este método primeiro tenta encontrar a empresa correspondente ao ID fornecido. Se a empresa não for encontrada, 
    /// notifica o usuário com uma mensagem de erro específica e retorna 0. Se a empresa for encontrada, 
    /// procede-se com a tentativa de exclusão. Se a exclusão for bem-sucedida, retorna 1, caso contrário, notifica o erro 
    /// e retorna 0. Este método lida com todas as exceções capturando-as e notificando o erro, retornando 0 nesses casos.
    /// </remarks>
    /// <param name="id">O ID da empresa a ser excluída.</param>
    /// <returns>
    /// Retorna 1 se a empresa for excluída com sucesso; 0 se a empresa não for encontrada, não for excluída ou ocorrer uma exceção.
    /// </returns>
    /// <exception cref="Exception">
    /// Captura e trata qualquer exceção que possa ocorrer durante o processo de localização ou exclusão da empresa,
    /// notificando o erro usando uma mensagem formatada e retornando 0.
    /// </exception>
    public async Task<int> DeleteAsync(int id)
    {
        try
        {
            var company = await _repositoryCompany.SelectById(id);
            if (company == null)
            {
                Notify(Resource.RecordNotFoundErrorMessage);
                return 0;
            }

            var saved = await _repositoryCompany.Delete(id);

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
    /// Processa a inserção ou atualização de endereços para uma empresa a partir de uma lista de objetos DTO.
    /// </summary>
    /// <param name="companyDTO">O objeto DTO <see cref="CompanyDTO"/> que contém os dados da empresa e uma lista de endereços.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona sem retorno, indicando a conclusão do processo de inserção ou atualização.
    /// </returns>
    /// <remarks>
    /// Este método separa os endereços contidos no 'companyDTO' em novos endereços e endereços a serem atualizados com base no valor de 'CompanyId'.
    /// Endereços com 'CompanyId' igual a zero são considerados novos e inseridos no banco de dados, enquanto endereços com 'CompanyId' diferente de zero
    /// são considerados existentes e submetidos a atualização.
    /// - Para cada endereço novo, o método <see cref="_serviceAddress.InsertAsync"/> é chamado.
    /// - Para cada endereço que necessita atualização, o método <see cref="_serviceAddress.UpdateAsync"/> é chamado.
    /// Este método assegura que todos os endereços associados à empresa sejam corretamente inseridos ou atualizados no banco de dados,
    /// facilitando a manutenção da consistência e integridade dos dados da empresa.
    /// </remarks>
    private async Task ProcessInsertOrUpdateAddress(CompanyDTO companyDTO)
    {
        List<AddressDTO> newAddressDTO = new List<AddressDTO>();
        List<AddressDTO> updateAddressDTO = new List<AddressDTO>();

        newAddressDTO = companyDTO.ListAddress.Where(x => x.AddressId == 0).ToList();
        updateAddressDTO = companyDTO.ListAddress.Where(x => x.AddressId != 0).ToList();

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
    /// Valida uma entidade Company usando um validador específico.
    /// </summary>
    /// <remarks>
    /// Este método aplica regras de validação definidas na classe `ValidationCompany` para uma entidade `Company`.
    /// A validação é realizada por uma instância da classe `ValidationCompany`, que contém a lógica específica
    /// para validar os campos da entidade Company. O método `RunValidation` é usado para executar a validação,
    /// retornando `true` se a entidade passar em todas as verificações ou `false` caso alguma verificação falhe.
    /// </remarks>
    /// <param name="company">A entidade Company a ser validada.</param>
    /// <returns>
    /// Retorna `true` se a entidade passar todas as validações; caso contrário, retorna `false`.
    /// </returns>
    /// <example>
    /// Exemplo de uso:
    /// <code>
    /// var company = new Company { Name = "Example Co.", CNPJ = "00.000.000/0001-00" };
    /// bool isValid = Validate(company);
    /// </code>
    /// </example>
    private bool Validate(Company company)
    {
        if (!RunValidation(new ValidationCompany(), company)) return false;

        return true;
    }


}
