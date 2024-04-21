using Leanwork.Rh.API.Controllers;
using Leanwork.Rh.Application;
using Leanwork.Rh.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Leanwork.Rh.API;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
public class CompaniesTechnologiesRelController : MainController
{
    private IServiceCompanyTechnologyRel _iServiceCompanyTechnologyRel;

    public CompaniesTechnologiesRelController(IServiceCompanyTechnologyRel iServiceCompanyTechnologyRel, INotificationError notificationError) : base(notificationError)
    {
        _iServiceCompanyTechnologyRel = iServiceCompanyTechnologyRel;
    }

    /// <summary>
    /// Cria uma nova relação entre empresa e tecnologia.
    /// </summary>
    /// <remarks>
    /// Este método recebe um objeto DTO representando a relação entre uma empresa e uma tecnologia e,
    /// se válido, insere essa relação no banco de dados. Caso haja problemas de validação ou na inserção,
    /// uma resposta personalizada com detalhes do problema é fornecida.
    /// </remarks>
    /// <param name="companyTechnologyRelDTO">Objeto DTO contendo as informações da relação entre empresa e tecnologia.</param>
    /// <returns>
    /// Retorna uma resposta com status 201 (Created) e detalhes da relação criada se a operação for bem-sucedida.
    /// Se a validação falhar ou a operação não for bem-sucedida, retorna uma resposta personalizada com status 400 (Bad Request).
    /// </returns>
    /// <response code="200">Relação criada com sucesso. Retorna os detalhes da relação.</response>
    /// <response code="400">Falha ao criar a relação. Retorna os detalhes do problema.</response>
    [ProducesResponseType(typeof(CompanyTechnologyRelDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CompanyTechnologyRelDTO companyTechnologyRelDTO)
    {
        if (!ModelState.IsValid) return CustomResponse(modelState: ModelState, title: "Requisição Inválida", method: "POST", url: $"api/{nameof(CompaniesTechnologiesRelController)}", statusCode: 400);

        await _iServiceCompanyTechnologyRel.InsertAsync(companyTechnologyRelDTO);

        if (!ValidOperation()) return CustomResponse(title: "Requisição Inválida", method: "POST", url: $"api/{nameof(CompaniesTechnologiesRelController)}", statusCode: 400);

        return CreatedAtAction(nameof(Post), new { companyTechnologyRelDTO.CompanyTechnologyRelId }, companyTechnologyRelDTO);
    }

    /// <summary>
    /// Exclui uma relação específica entre empresa e tecnologia com base no identificador fornecido.
    /// </summary>
    /// <remarks>
    /// Este método exclui uma relação entre empresa e tecnologia identificada pelo ID.
    /// Se a operação de exclusão for bem-sucedida, retorna um status 204 (No Content).
    /// Caso a operação falhe ou o ID não exista, retorna uma resposta personalizada com status 400 (Bad Request) ou 404 (Not Found), respectivamente.
    /// </remarks>
    /// <param name="id">Identificador da relação entre empresa e tecnologia a ser excluída.</param>
    /// <returns>
    /// Retorna NoContent (204) se a exclusão for bem-sucedida.
    /// Retorna BadRequest (400) se ocorrer algum erro durante a operação de exclusão.
    /// Retorna NotFound (404) se a relação especificada não for encontrada.
    /// </returns>
    /// <response code="200">A relação foi excluída com sucesso. Retorna status sem conteúdo.</response>
    /// <response code="400">Falha ao excluir a relação. Retorna os detalhes do problema.</response>
    /// <response code="404">Não foi encontrada uma relação com o ID fornecido. Retorna os detalhes do problema.</response>
    [ProducesResponseType(typeof(CompanyTechnologyRelDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _iServiceCompanyTechnologyRel.DeleteAsync(id);

        if (!ValidOperation()) return CustomResponse(title: "Requisição Inválida", method: "DELTE", url: $"api/{nameof(CompaniesTechnologiesRelController)}", statusCode: 400);

        return NoContent();
    }

}
