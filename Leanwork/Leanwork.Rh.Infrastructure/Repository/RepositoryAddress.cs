using Leanwork.Rh.Domain;
using Leanwork.Rh.Domain.Interface;

namespace Leanwork.Rh.Infrastructure;

public class RepositoryAddress : IRepositoryAddress
{
    private readonly ISqlDataAccess _access;

    public RepositoryAddress(ISqlDataAccess access)
    {
        _access = access;
    }

    /// <summary>
    /// Recupera de forma assíncrona todos os endereços associados a uma empresa específica do banco de dados.
    /// </summary>
    /// <param name="id">O identificador da empresa cujos endereços serão recuperados.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e retorna uma coleção enumerável de objetos <see cref="Address"/>
    /// contendo os endereços da empresa especificada.
    /// </returns>
    /// <remarks>
    /// Este método utiliza uma consulta parametrizada para buscar todos os endereços relacionados a uma empresa específica,
    /// identificada pelo parâmetro 'id'. A consulta é realizada de forma assíncrona para melhorar a performance e não bloquear
    /// a thread principal durante a execução. Os resultados são mapeados para uma lista de objetos 'Address' e retornados ao chamador.
    /// É importante garantir que o 'id' fornecido corresponda a uma empresa válida no banco de dados para evitar resultados vazios.
    /// </remarks>
    public async Task<IEnumerable<Address>> SelectAllByCompany(int id)
    {
        var parameters = new { Id = id };
        var data = await _access.QueryAsync<Address, dynamic>(QueryAddress.SelectAllByCompany, parameters);

        return data;
    }

    /// <summary>
    /// Recupera de forma assíncrona todos os endereços associados a um candidato específico do banco de dados.
    /// </summary>
    /// <param name="id">O identificador do candidato cujos endereços serão recuperados.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e retorna uma coleção enumerável de objetos <see cref="Address"/>
    /// contendo os endereços do candidato especificado.
    /// </returns>
    /// <remarks>
    /// Este método utiliza uma consulta parametrizada para buscar todos os endereços relacionados a um candidato específico,
    /// identificado pelo parâmetro 'id'. A consulta é realizada de forma assíncrona para melhorar a eficiência da execução e 
    /// minimizar o bloqueio da thread principal. Os resultados são mapeados para uma lista de objetos 'Address' e retornados ao 
    /// chamador. Assegure-se de que o 'id' fornecido corresponda a um candidato válido no banco de dados para evitar resultados vazios.
    /// Este método é útil para obter rapidamente uma visão completa dos endereços associados a um candidato, o que pode ser 
    /// essencial para processos de verificação de informações ou logística relacionada ao candidato.
    /// </remarks>
    public async Task<IEnumerable<Address>> SelectAllByCandidate(int id)
    {
        var parameters = new { Id = id };
        var data = await _access.QueryAsync<Address, dynamic>(QueryAddress.SelectAllByCandidate, parameters);

        return data;
    }

    /// <summary>
    /// Recupera de forma assíncrona um endereço específico do banco de dados pelo seu identificador.
    /// </summary>
    /// <param name="id">O identificador do endereço a ser recuperado.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona, retornando o objeto <see cref="Address"/> correspondente ao identificador fornecido,
    /// ou <c>null</c> se nenhum endereço for encontrado.
    /// </returns>
    /// <remarks>
    /// Este método executa uma consulta ao banco de dados para buscar um endereço específico pela coluna de identificação.
    /// Utiliza o método 'QueryAsync' do Dapper com parâmetros dinâmicos para especificar o ID desejado. A consulta é feita de forma
    /// assíncrona para melhorar a performance e evitar bloqueios da thread principal. O método retorna o primeiro objeto encontrado
    /// na consulta ou null se a consulta não retornar nenhum resultado.
    /// Este método é especialmente útil para verificar detalhes de um endereço específico rapidamente, como parte de processos de verificação
    /// de informações ou operações logísticas que necessitam de dados de localização precisos.
    /// </remarks>
    public async Task<Address> SelectById(int id)
    {
        var parameters = new { Id = id };
        var data = await _access.QueryAsync<Address, dynamic>(QueryAddress.SelectById, parameters);

        return data.FirstOrDefault();
    }

    /// <summary>
    /// Insere um novo endereço no banco de dados de forma assíncrona.
    /// </summary>
    /// <param name="address">O objeto <see cref="Address"/> que contém os dados do endereço a ser inserido.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e retorna um inteiro indicando o número de registros afetados pela inserção.
    /// Retorna um valor maior que zero se a inserção for bem-sucedida, ou zero em caso de falha.
    /// </returns>
    /// <remarks>
    /// Este método configura os parâmetros do endereço para passá-los a um procedimento armazenado que realiza a inserção no banco de dados.
    /// Os dados do endereço incluem identificadores de empresa e candidato, nome do endereço, CEP, rua, número, complemento, bairro, cidade,
    /// estado e datas de criação e modificação. Em caso de sucesso, o procedimento armazenado retorna o número de registros afetados. Se uma exceção
    /// é capturada durante a operação, o método retorna zero, indicando que a inserção falhou.
    /// </remarks>
    public async Task<int> Insert(Address address)
    {
        try
        {
            var parameters = new
            {
                address.CompanyId,
                address.CandidateId,
                address.Name,
                address.ZipCode,
                address.Street,
                address.Number,
                address.Complement,
                address.Neighborhood,
                address.City,
                address.State,
                address.CreationDate,
                address.ModificationDate
            };

            // Executa o procedimento armazenado e captura o resultado
            var success = await _access.SaveData(QueryAddress.Insert, parameters);

            // Retorna o resultado
            return success;
        }
        catch (Exception ex)
        {
            return 0; // Retorna falso em caso de erro
        }
    }

    /// <summary>
    /// Atualiza os dados de um endereço existente no banco de dados de forma assíncrona.
    /// </summary>
    /// <param name="address">O objeto <see cref="Address"/> que contém os dados atualizados do endereço.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e retorna um inteiro indicando o número de registros afetados pela atualização.
    /// Retorna um valor maior que zero se a atualização for bem-sucedida, ou zero em caso de falha.
    /// </returns>
    /// <remarks>
    /// Este método configura e envia os dados atualizados do endereço para um procedimento armazenado, que realiza a atualização no banco de dados.
    /// Os dados enviados incluem o identificador do endereço, identificadores de empresa e candidato, nome, CEP, rua, número, complemento, bairro,
    /// cidade, estado e a data de modificação. O procedimento verifica se o endereço especificado existe e atualiza suas informações conforme fornecido.
    /// Em caso de sucesso, o procedimento armazenado retorna o número de registros afetados. Se uma exceção é capturada durante a operação, o método
    /// retorna zero, indicando que a atualização falhou devido a um erro técnico ou de validação dos dados.
    /// </remarks>
    public async Task<int> Update(Address address)
    {
        try
        {
            var parameters = new
            {
                address.AddressId,
                address.CompanyId,
                address.CandidateId,
                address.Name,
                address.ZipCode,
                address.Street,
                address.Number,
                address.Complement,
                address.Neighborhood,
                address.City,
                address.State,
                address.ModificationDate
            };

            var success = await _access.SaveData(QueryAddress.Update, parameters);

            return success;

        }
        catch (Exception ex)
        {
            return 0; // Retorna falso em caso de erro
        }
    }

    /// <summary>
    /// Exclui um endereço do banco de dados de forma assíncrona utilizando seu identificador.
    /// </summary>
    /// <param name="id">O identificador do endereço a ser excluído.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e retorna um inteiro indicando o número de registros afetados pela exclusão.
    /// Retorna um valor maior que zero se a exclusão for bem-sucedida, ou zero em caso de falha.
    /// </returns>
    /// <remarks>
    /// Este método utiliza um procedimento armazenado para excluir um endereço específico do banco de dados, identificado pelo 'AddressId' fornecido.
    /// A exclusão é feita de forma assíncrona para evitar bloqueios da thread principal e para melhorar a eficiência do processo. O procedimento armazenado
    /// verifica a existência do endereço e, se encontrado, procede com a exclusão. Se uma exceção é capturada durante a operação ou se o endereço não
    /// puder ser excluído devido a restrições de chave estrangeira ou outras regras de negócio, o método retorna zero, indicando a falha na exclusão.
    /// </remarks>
    public async Task<int> Delete(int id)
    {
        try
        {
            var parameters = new { AddressId = id };
            var success = await _access.SaveData(QueryAddress.Delete, parameters);

            return success;
        }
        catch (Exception ex)
        {
            return 0; // Retorna falso em caso de erro
        }
    }

    /// <summary>
    /// Exclui todos os endereços associados a uma empresa específica de forma assíncrona.
    /// </summary>
    /// <param name="id">O identificador da empresa cujos endereços serão excluídos.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e retorna um inteiro indicando o número de registros afetados pela exclusão.
    /// Retorna um valor maior que zero se a exclusão for bem-sucedida, ou zero em caso de falha.
    /// </returns>
    /// <remarks>
    /// Este método utiliza um procedimento armazenado especificado em 'QueryAddress.DeleteAllCompany' para excluir todos os endereços
    /// vinculados ao 'CompanyId' fornecido. Os parâmetros necessários para a execução da query são passados diretamente ao método 'SaveData'
    /// do objeto '_access', que executa a operação de exclusão no banco de dados.
    /// Este método é útil para operações de limpeza de dados ou quando uma empresa é removida do sistema, garantindo que todos os endereços
    /// associados também sejam removidos para manter a integridade dos dados.
    /// Em caso de falha na execução do procedimento armazenado ou se ocorrer uma exceção, o método retorna zero, indicando que a operação falhou.
    /// </remarks>
    public async Task<int> DeleteAllCompanyAsync(int id)
    {
        try
        {
            var parameters = new { CompanyId = id };
            var success = await _access.SaveData(QueryAddress.DeleteAllCompany, parameters);

            return success;
        }
        catch (Exception ex)
        {
            return 0; // Retorna falso em caso de erro
        }
    }

    /// <summary>
    /// Exclui todos os endereços associados a um candidato específico de forma assíncrona.
    /// </summary>
    /// <param name="id">O identificador do candidato cujos endereços serão excluídos.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona e retorna um inteiro indicando o número de registros afetados pela exclusão.
    /// Retorna um valor maior que zero se a exclusão for bem-sucedida, ou zero em caso de falha.
    /// </returns>
    /// <remarks>
    /// Este método utiliza um procedimento armazenado especificado em 'QueryAddress.DeleteAllCandidate' para excluir todos os endereços
    /// vinculados ao 'CandidateId' fornecido. Os parâmetros necessários para a execução da query são passados diretamente ao método 'SaveData'
    /// do objeto '_access', que executa a operação de exclusão no banco de dados.
    /// Este método é útil para operações de limpeza de dados ou quando um candidato é removido do sistema, garantindo que todos os endereços
    /// associados também sejam removidos para manter a integridade dos dados.
    /// Em caso de falha na execução do procedimento armazenado ou se ocorrer uma exceção, o método retorna zero, indicando que a operação falhou.
    /// </remarks>
    public async Task<int> DeleteAllCandidateAsync(int id)
    {
        try
        {
            var parameters = new { CandidateId = id };
            var success = await _access.SaveData(QueryAddress.DeleteAllCandidate, parameters);

            return success;
        }
        catch (Exception ex)
        {
            return 0; // Retorna falso em caso de erro
        }
    }
    


}
