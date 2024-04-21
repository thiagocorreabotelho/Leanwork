using Leanwork.Rh.Domain;
using Leanwork.Rh.Domain.Interface;

namespace Leanwork.Rh.Infrastructure;

public class RepositoryCompany : IRepositoryCompany
{
    private readonly ISqlDataAccess _access;

    public RepositoryCompany(ISqlDataAccess access)
    {
        _access = access;
    }

    /// <summary>
    /// Seleciona todas as empresas registradas no banco de dados.
    /// </summary>
    /// <remarks>
    /// Este método executa uma consulta SQL definida em `QueryCompany.SelectAll` para recuperar todos os registros da tabela de empresas.
    /// Utiliza Dapper para executar a consulta de forma assíncrona, retornando uma lista de objetos `Company`.
    /// </remarks>
    /// <returns>
    /// Uma tarefa que, quando concluída com sucesso, retorna uma coleção enumerável de objetos `Company`.
    /// A coleção pode estar vazia se não houver empresas registradas no banco de dados.
    /// </returns>
    /// <exception cref="System.Data.SqlClient.SqlException">
    /// Lançada quando ocorre um erro durante a execução da consulta no banco de dados.
    /// </exception>
    /// <exception cref="System.InvalidOperationException">
    /// Lançada se a consulta não puder ser executada ou se o resultado não puder ser convertido para o tipo `Company`.
    /// </exception>
    public async Task<IEnumerable<Company>> SelectAll()
    {
        var data = await _access.QueryAsync<Company, dynamic>(QueryCompany.SelectAll, new { });

        return data;
    }

    /// <summary>
    /// Recupera uma empresa específica pelo seu ID.
    /// </summary>
    /// <remarks>
    /// Este método consulta a tabela de empresas usando uma consulta SQL específica, definida em `QueryCompany.SelectById`, para encontrar uma empresa pelo ID fornecido.
    /// A consulta é executada de forma assíncrona utilizando Dapper para mapear o resultado para um objeto `Company`.
    /// </remarks>
    /// <param name="id">O ID da empresa a ser recuperada.</param>
    /// <returns>
    /// Uma tarefa que, quando concluída com sucesso, retorna o objeto `Company` correspondente ao ID fornecido.
    /// Retorna null se nenhuma empresa for encontrada com esse ID.
    /// </returns>
    /// <exception cref="System.Data.SqlClient.SqlException">
    /// Lançada quando ocorre um erro durante a execução da consulta ao banco de dados.
    /// </exception>
    /// <exception cref="System.ArgumentNullException">
    /// Lançada se o parâmetro `id` for passado como null.
    /// </exception>
    /// <exception cref="System.InvalidOperationException">
    /// Lançada se o resultado da consulta não puder ser convertido para o tipo `Company`.
    /// </exception>
    public async Task<Company> SelectById(int id)
    {
        var parameters = new { Id = id };
        var data = await _access.QueryAsync<Company, dynamic>(QueryCompany.SelectById, parameters);

        return data.FirstOrDefault();
    }

    /// <summary>
    /// Insere um novo registro de empresa no banco de dados.
    /// </summary>
    /// <remarks>
    /// Este método adiciona uma nova empresa ao banco de dados usando um procedimento armazenado especificado em `QueryCompany.Insert`.
    /// Ele utiliza um objeto `Company` para passar os dados da empresa ao procedimento armazenado. Os parâmetros incluem nome, CNPJ, data de abertura, 
    /// email, data de criação e data de modificação.
    /// O método retorna um inteiro indicando o resultado da operação, onde um valor maior que zero representa o número de registros afetados, 
    /// sugerindo sucesso na inserção. Um retorno de zero indica que nenhum registro foi inserido, geralmente devido a uma violação de regras de negócio ou dados inválidos.
    /// </remarks>
    /// <param name="company">O objeto `Company` contendo os dados da empresa a ser inserida.</param>
    /// <returns>
    /// Um inteiro que indica o número de registros afetados pela operação de inserção. Retorna 0 em caso de falha na inserção.
    /// </returns>
    /// <exception cref="Exception">
    /// Uma exceção genérica é capturada e tratada dentro do método, retornando 0 em caso de erros na operação de banco de dados ou lógica de negócios.
    /// </exception>
    public async Task<int> Insert(Company company)
    {
        try
        {
            var parameters = new
            {
                company.Name,
                company.CNPJ,
                company.OpenDate,
                company.Email,
                company.CreationDate,
                company.ModificationDate,
            };

            // Executa o procedimento armazenado e captura o resultado
            var success = await _access.SaveData(QueryCompany.Insert, parameters);

            // Retorna o resultado
            return success;
        }
        catch (Exception ex)
        {
            return 0; // Retorna falso em caso de erro
        }
    }

    /// <summary>
    /// Atualiza os dados de uma empresa existente no banco de dados.
    /// </summary>
    /// <remarks>
    /// Este método atualiza uma empresa com base no ID fornecido através do objeto `Company`. Os campos atualizáveis incluem o nome, CNPJ, data de abertura, 
    /// email e data de modificação. A atualização é realizada por meio de um procedimento armazenado especificado em `QueryCompany.Update`.
    /// 
    /// O retorno é um inteiro indicando o número de linhas afetadas pela operação. Um retorno de 0 indica que nenhuma linha foi afetada, 
    /// o que pode ocorrer se a empresa não existir ou se os dados fornecidos não alterarem de fato os valores existentes.
    /// </remarks>
    /// <param name="company">O objeto `Company` contendo os dados atualizados da empresa, incluindo seu ID.</param>
    /// <returns>
    /// Um inteiro que indica o número de registros afetados pela operação de atualização. Retorna 0 em caso de falha na atualização.
    /// </returns>
    /// <exception cref="Exception">
    /// Uma exceção genérica é capturada e tratada dentro do método, retornando 0 em caso de erros durante a operação de atualização.
    /// </exception>
    public async Task<int> Update(Company company)
    {
        try
        {
            var parameters = new
            {
                company.CompanyId,
                company.Name,
                company.CNPJ,
                company.OpenDate,
                company.Email,
                company.ModificationDate,
            };

            var success = await _access.SaveData(QueryCompany.Update, parameters);

            return success;

        }
        catch (Exception ex)
        {
            return 0; // Retorna falso em caso de erro
        }
    }

    /// <summary>
    /// Exclui uma empresa do banco de dados com base em seu ID.
    /// </summary>
    /// <remarks>
    /// Este método remove uma empresa utilizando um procedimento armazenado especificado em `QueryCompany.Delete`. 
    /// O ID da empresa a ser excluída é passado como parâmetro para o procedimento armazenado. A operação retorna o número de registros afetados, 
    /// permitindo verificar se a exclusão foi bem-sucedida.
    /// 
    /// Um retorno de 0 indica que nenhum registro foi afetado, o que normalmente acontece se o ID fornecido não corresponder a nenhuma empresa existente.
    /// </remarks>
    /// <param name="id">O ID da empresa a ser excluída.</param>
    /// <returns>
    /// Um inteiro que indica o número de registros afetados pela operação de exclusão. Retorna 0 em caso de falha na exclusão.
    /// </returns>
    /// <exception cref="Exception">
    /// Uma exceção genérica é capturada e tratada dentro do método, retornando 0 em caso de erros durante a operação de exclusão.
    /// </exception>
    public async Task<int> Delete(int id)
    {
        try
        {
            var parameters = new { CompanyId = id };
            var success = await _access.SaveData(QueryCompany.Delete, parameters);

            return success;
        }
        catch (Exception ex)
        {
            return 0; // Retorna falso em caso de erro
        }
    }

}


