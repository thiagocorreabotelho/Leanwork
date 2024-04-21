using Leanwork.Rh.Domain;
using Leanwork.Rh.Domain.Interface;

namespace Leanwork.Rh.Infrastructure;

public class RepositoryCompanyTechnologyRel : IRepositoryCompanyTechnologyRel
{
    private readonly ISqlDataAccess _access;

    public RepositoryCompanyTechnologyRel(ISqlDataAccess access)
    {
        _access = access;
    }

    /// <summary>
    /// Seleciona todas as tecnologias associadas a uma empresa específica.
    /// </summary>
    /// <remarks>
    /// Este método consulta o banco de dados para recuperar todas as tecnologias associadas a uma empresa, identificada pelo seu ID.
    /// A consulta é realizada utilizando uma instrução SQL específica armazenada na constante 'QueryCompanyTechnologyRel.SelectAllTechnologyByCompany'
    /// e passando o ID da empresa como parâmetro.
    /// </remarks>
    /// <param name="id">O identificador da empresa para qual as tecnologias são solicitadas.</param>
    /// <returns>
    /// Retorna uma coleção de entidades 'CompanyTechnologyRel' que representam as tecnologias associadas à empresa especificada.
    /// </returns>
    public async Task<IEnumerable<CompanyTechnologyRel>> SelectAllTechnologiesByCompany(int id)
    {
        var parameters = new { Id = id };
        var data = await _access.QueryAsync<CompanyTechnologyRel, dynamic>(QueryCompanyTechnologyRel.SelectAllTechnologyByCompany, parameters);

        return data;
    }

    public async Task<int> Insert(CompanyTechnologyRel companyTechnologyRel)
    {
        try
        {
            var parameters = new
            {
                companyTechnologyRel.CompanyId,
                companyTechnologyRel.TechnologyId,
                companyTechnologyRel.CreationDate,
                companyTechnologyRel.ModificationDate,
            };

            // Executa o procedimento armazenado e captura o resultado
            var success = await _access.SaveData(QueryCompanyTechnologyRel.Insert, parameters);

            // Retorna o resultado
            return success;
        }
        catch (Exception ex)
        {
            return 0; // Retorna falso em caso de erro
        }
    }

    /// <summary>
    /// Insere uma nova relação de tecnologia de empresa no banco de dados.
    /// </summary>
    /// <remarks>
    /// Este método tenta inserir uma nova relação entre uma empresa e uma tecnologia usando um procedimento armazenado.
    /// Os parâmetros necessários para a inserção (ID da empresa, ID da tecnologia, data de criação e data de modificação)
    /// são extraídos da entidade 'CompanyTechnologyRel' e passados ao procedimento armazenado.
    /// Em caso de sucesso, o método retorna o número de registros afetados.
    /// </remarks>
    /// <param name="companyTechnologyRel">A entidade contendo os dados da relação de tecnologia de empresa a ser inserida.</param>
    /// <returns>
    /// Retorna o número de registros afetados pela operação de inserção. Retorna 0 em caso de falha.
    /// </returns>
    /// <exception cref="Exception">Captura qualquer exceção que ocorra durante o processo de inserção e retorna 0.</exception>
    public async Task<int> Delete(int id)
    {
        try
        {
            var parameters = new { CompanyTechnologyId = id };
            var success = await _access.SaveData(QueryCompanyTechnologyRel.Delete, parameters);

            return success;
        }
        catch (Exception ex)
        {
            return 0; // Retorna falso em caso de erro
        }
    }
}
