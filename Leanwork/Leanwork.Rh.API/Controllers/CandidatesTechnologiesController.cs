using Leanwork.Rh.API.Controllers;
using Leanwork.Rh.Application;
using Leanwork.Rh.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Leanwork.Rh.API;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
public class CandidatesTechnologiesController : MainController
{
    private IServiceCandidateTechnologyRel _serviceCandidateTechnologyRel;

    public CandidatesTechnologiesController(IServiceCandidateTechnologyRel serviceCandidateTechnologyRel, INotificationError notificationError) : base(notificationError)
    {
        _serviceCandidateTechnologyRel = serviceCandidateTechnologyRel;
    }

    /// <summary>
    /// Cria um novo registro de relacionamento entre candidato e tecnologia com base nos dados fornecidos.
    /// </summary>
    /// <param name="candidateTechnologyRelDTO">O DTO de CandidateTechnologyRel contendo os dados necessários para criar o relacionamento.</param>
    /// <returns>
    /// Retorna uma resposta de ação criada (HTTP 201) com o DTO inserido se a operação for bem-sucedida.
    /// Retorna uma resposta personalizada com detalhes do problema (HTTP 400) se a validação dos dados falhar ou se a operação de inserção não for bem-sucedida.
    /// </returns>
    /// <remarks>
    /// Este método valida o estado do modelo antes de prosseguir com qualquer operação de inserção. Se o estado do modelo não for válido,
    /// a função retorna imediatamente uma resposta de erro personalizada.
    /// </remarks>
    [ProducesResponseType(typeof(CandidateTechnologyRelDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CandidateTechnologyRelDTO candidateTechnologyRelDTO)
    {
        if (!ModelState.IsValid) return CustomResponse(modelState: ModelState, title: "Requisição Inválida", method: "POST", url: "api/Genders", statusCode: 400);

        await _serviceCandidateTechnologyRel.InsertAsync(candidateTechnologyRelDTO);

        if (!ValidOperation()) return CustomResponse(title: "Requisição Inválida", method: "POST", url: $"api/{nameof(CandidatesTechnologiesController)}", statusCode: 400);

        return CreatedAtAction(nameof(Post), new { candidateTechnologyRelDTO.CandidateTechnologyRelId }, candidateTechnologyRelDTO);
    }

    /// <summary>
    /// Deletes a candidate-technology relationship based on the provided ID.
    /// </summary>
    /// <param name="id">The ID of the candidate-technology relationship to delete.</param>
    /// <returns>
    /// An ActionResult representing the outcome of the operation. Returns a No Content (HTTP 204) response if the delete operation
    /// is successful. Returns a Bad Request (HTTP 400) with details of the problem if the operation fails due to business logic conditions.
    /// </returns>
    /// <remarks>
    /// This method calls an asynchronous delete operation on the service layer. It validates the operation result, and if the operation
    /// fails to meet business validation rules, it returns a customized error response. It also handles not found scenarios implicitly
    /// by checking operation validity.
    /// </remarks>
    [ProducesResponseType(typeof(CandidateTechnologyRelDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _serviceCandidateTechnologyRel.DeleteAsync(id);

        if (!ValidOperation()) return CustomResponse(title: "Requisição Inválida", method: "DELTE", url: $"api/{nameof(CandidatesTechnologiesController)}", statusCode: 400);

        return NoContent();
    }
}
