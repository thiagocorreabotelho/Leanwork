using Leanwork.Rh.Application;
using Leanwork.Rh.Application.DTO.Interview;
using Leanwork.Rh.Application.Interface;
using Leanwork.Rh.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Leanwork.Rh.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
public class InterviewsController : MainController
{
    IServiceInterview _iServiceInterview;

    public InterviewsController(IServiceInterview iServiceInterview, INotificationError notificationError) : base(notificationError)
    {
        _iServiceInterview = iServiceInterview;
    }

    /// <summary>
    /// Cria uma nova entrevista.
    /// </summary>
    /// <remarks>
    /// Este endpoint recebe uma entrevista em formato JSON e a insere no banco de dados.
    /// Caso a validação falhe ou a inserção não seja bem-sucedida, retorna um erro 400 com detalhes do problema.
    /// </remarks>
    /// <param name="interviewDTO">O DTO da entrevista que contém os dados necessários para a criação da entrevista.</param>
    /// <returns>Retorna um status 201 e os detalhes da entrevista criada ou um status 400 em caso de erro.</returns>
    /// <response code="201">Retorna os detalhes da entrevista criada.</response>
    /// <response code="400">Retorna detalhes do erro se a validação ou inserção falharem.</response>
    [ProducesResponseType(typeof(InterviewDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] InterviewDTO interviewDTO)
    {
        if (!ModelState.IsValid) return CustomResponse(modelState: ModelState, title: "Requisição Inválida", method: "POST", url: $"api/{nameof(InterviewsController)}", statusCode: 400);

        await _iServiceInterview.Insert(interviewDTO);

        if (!ValidOperation()) return CustomResponse(title: "Requisição Inválida", method: "POST", url: $"api/{nameof(InterviewsController)}", statusCode: 400);

        return CreatedAtAction(nameof(Post), new { interviewDTO.InterviewId }, interviewDTO);
    }

    /// <summary>
    /// Deleta uma entrevista existente pelo ID.
    /// </summary>
    /// <param name="id">O identificador da entrevista a ser deletada.</param>
    /// <returns>Retorna NoContent se a entrevista for deletada com sucesso; caso contrário, retorna um erro com o status apropriado.</returns>
    /// <remarks>
    /// Este endpoint deleta uma entrevista com base no ID fornecido. Se a entrevista não puder ser deletada devido a uma falha de validação ou se não for encontrada,
    /// uma mensagem de erro é retornada.
    /// </remarks>
    /// <response code="204">Retorna NoContent se a entrevista for deletada com sucesso.</response>
    /// <response code="400">Retorna BadRequest se houver uma falha de validação.</response>
    /// <response code="404">Retorna NotFound se nenhuma entrevista com o ID fornecido for encontrada.</response>
    [ProducesResponseType(typeof(InterviewDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _iServiceInterview.Delete(id);

        if (!ValidOperation()) return CustomResponse(title: "Requisição Inválida", method: "DELTE", url: $"api/{nameof(InterviewsController)}", statusCode: 400);

        return NoContent();
    }
}
