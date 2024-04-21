using Leanwork.Rh.API.Controllers;
using Leanwork.Rh.Application;
using Leanwork.Rh.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Leanwork.Rh.API;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
public class JobsOpeningsController : MainController
{
    private IServiceJobOpening _iServiceJobOpening;

    public JobsOpeningsController(IServiceJobOpening iServiceJobOpening, INotificationError notificationError) : base(notificationError)
    {
        _iServiceJobOpening = iServiceJobOpening;
    }

    /// <summary>
    /// Obtém uma lista de todas as vagas de emprego disponíveis.
    /// </summary>
    /// <remarks>
    /// Este endpoint retorna uma lista de vagas de emprego, mapeadas para o formato DTO.
    /// </remarks>
    /// <response code="200">Retorna a lista das vagas de emprego disponíveis. A lista pode estar vazia se não houver vagas disponíveis.</response>
    /// <response code="500">Ocorreu um erro interno no servidor ao tentar processar a solicitação.</response>

    [ProducesResponseType(typeof(List<JobOpeningDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("All")]
    public async Task<ActionResult<IEnumerable<JobOpeningDTO>>> GetList()
    {
        var getList = await _iServiceJobOpening.SelectAllAsync();
        return Ok(getList);
    }

    /// <summary>
    /// Obtém a lista de todas as vagas de emprego disponíveis.
    /// </summary>
    /// <remarks>
    /// Este endpoint retorna todas as vagas de emprego disponíveis como uma lista de DTOs.
    /// É útil para obter um resumo rápido de todas as vagas abertas sem detalhes específicos.
    /// </remarks>
    /// <response code="200">Retorna a lista de vagas de emprego disponíveis</response>
    /// <response code="500">Erro interno no servidor</response>
    [ProducesResponseType(typeof(List<JobOpeningDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("Available")]
    public async Task<ActionResult<IEnumerable<JobOpeningDTO>>> GetListByAvailable()
    {
        var getList = await _iServiceJobOpening.SelectAllAsync();
        return Ok(getList);
    }

    /// <summary>
    /// Obtém uma vaga de emprego específica pelo seu ID.
    /// </summary>
    /// <param name="id">O identificador único da vaga de emprego.</param>
    /// <remarks>
    /// Este endpoint retorna uma vaga de emprego específica, mapeada para o formato DTO. Se nenhuma vaga for encontrada com o ID fornecido, retorna 404 Not Found.
    /// </remarks>
    /// <response code="200">Retorna a vaga de emprego especificada se encontrada.</response>
    /// <response code="404">Nenhuma vaga de emprego foi encontrada para o ID especificado.</response>
    [ProducesResponseType(typeof(List<JobOpeningDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<JobOpeningDTO>> GetById(int id)
    {
        var getCompanyById = await _iServiceJobOpening.SelectByIdAsync(id);

        return Ok(getCompanyById);
    }

    /// <summary>
    /// Cria uma nova vaga de emprego com os dados fornecidos.
    /// </summary>
    /// <param name="jobOpeningDTO">O DTO da vaga de emprego que contém os dados necessários para a criação.</param>
    /// <remarks>
    /// Este endpoint recebe um DTO de vaga de emprego, valida esse DTO e, se válido, insere a vaga no repositório.
    /// Retorna 400 Bad Request se o DTO não for válido ou se a operação de inserção falhar por qualquer motivo.
    /// </remarks>
    /// <response code="201">Retorna a vaga de emprego criada com o respectivo ID.</response>
    /// <response code="400">Dados inválidos foram fornecidos ou ocorreu um erro durante a inserção da vaga de emprego.</response>
    [ProducesResponseType(typeof(JobOpeningDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] JobOpeningDTO jobOpeningDTO)
    {
        if (!ModelState.IsValid) return CustomResponse(modelState: ModelState, title: "Requisição Inválida", method: "POST", url: $"api/{nameof(JobsOpeningsController)}", statusCode: 400);

        await _iServiceJobOpening.InsertAsync(jobOpeningDTO);

        if (!ValidOperation()) return CustomResponse(title: "Requisição Inválida", method: "POST", url: $"api/{nameof(JobsOpeningsController)}", statusCode: 400);

        return CreatedAtAction(nameof(Post), new { jobOpeningDTO.JobOpeningId }, jobOpeningDTO);
    }

    /// <summary>
    /// Atualiza uma vaga de emprego existente com base no ID fornecido e nos dados enviados.
    /// </summary>
    /// <param name="id">O identificador único da vaga de emprego que será atualizada.</param>
    /// <param name="jobOpeningDTO">O DTO da vaga de emprego contendo os dados atualizados.</param>
    /// <remarks>
    /// Este endpoint atualiza uma vaga de emprego. Ele verifica se o ID fornecido no caminho da URL corresponde ao ID no DTO.
    /// Se os IDs não coincidirem, uma resposta de 400 Bad Request é retornada.
    /// Se não encontrar a vaga pelo ID, retorna 404 Not Found.
    /// Se houver erro na validação da operação após a tentativa de atualização, também retorna 400 Bad Request.
    /// </remarks>
    /// <response code="200">Retorna a vaga de emprego atualizada.</response>
    /// <response code="400">Erro de validação do ID ou outro erro durante a atualização.</response>
    /// <response code="404">Nenhuma vaga de emprego encontrada para o ID especificado.</response>
    [ProducesResponseType(typeof(JobOpeningDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<JobOpeningDTO>> Put(int id, [FromBody] JobOpeningDTO jobOpeningDTO)
    {
        if (id != jobOpeningDTO.JobOpeningId)
        {
            NotificationError("Ocorreu um erro durante a tentativa de atualização do registro: o ID fornecido na solicitação não corresponde ao ID do objeto em questão");
            return CustomResponse(title: "Requisição Invalida", method: "PUT", url: $"api/{nameof(JobsOpeningsController)}", statusCode: 400);
        }

        var getById = await _iServiceJobOpening.SelectByIdAsync(id);

        if (getById is null)
        {
            return NotFound(new
            {
                Instance = $"api/{nameof(JobsOpeningsController)}",
                status = 404,
                error = "Chamada não localizada",
                message = "Ocorreu um erro ao tentar localizar sua chamada",
                Method = "PUT",
            });
        }

        await _iServiceJobOpening.UpdateAsync(jobOpeningDTO);

        if (!ValidOperation()) return CustomResponse(title: "Requisição Invalida", method: "PUT", url: $"api/{nameof(JobsOpeningsController)}", statusCode: 400);

        return Ok(jobOpeningDTO);
    }

    /// <summary>
    /// Remove uma vaga de emprego existente com base no ID fornecido.
    /// </summary>
    /// <param name="id">O identificador único da vaga de emprego a ser removida.</param>
    /// <remarks>
    /// Este endpoint exclui uma vaga de emprego existente. Verifica se a vaga existe antes da exclusão.
    /// Retorna 404 Not Found se a vaga não for encontrada pelo ID especificado.
    /// Se ocorrer um erro durante a operação de exclusão, uma resposta de 400 Bad Request é retornada.
    /// </remarks>
    /// <response code="204">A vaga de emprego foi excluída com sucesso.</response>
    /// <response code="400">Erro durante a operação de exclusão.</response>
    /// <response code="404">Nenhuma vaga de emprego encontrada para o ID especificado.</response>
    [ProducesResponseType(typeof(JobOpeningDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var getById = await _iServiceJobOpening.SelectByIdAsync(id);

        if (getById is null)
        {
            return NotFound(new
            {
                Instance = $"api/{nameof(JobsOpeningsController)}/{id}",
                status = 404,
                error = "Chamada não encontrada",
                message = "Ocorreu um erro ao tentar localizar sua chamada.",
                Method = "DELETE",
            });
        }

        await _iServiceJobOpening.DeleteAsync(id);

        if (!ValidOperation()) return CustomResponse(title: "Requisição Inválida", method: "DELTE", url: $"api/{nameof(JobsOpeningsController)}", statusCode: 400);

        return NoContent();
    }

}
