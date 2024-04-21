using Leanwork.Rh.Application.DTO.Technology;
using Leanwork.Rh.Application.Interface;
using Leanwork.Rh.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Leanwork.Rh.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
public class TechnologysController : MainController
{
    private readonly IServiceTechnology _serviceTechnology;

    public TechnologysController(IServiceTechnology serviceTechnology, INotificationError notificationError) : base(notificationError)
    {
        _serviceTechnology = serviceTechnology;
    }

    /// <summary>
    /// Obtém uma lista de todas as tecnologias registradas no sistema.
    /// </summary>
    /// <remarks>
    /// Este endpoint realiza uma consulta para obter todos os registros de tecnologias existentes no banco de dados.
    /// Retorna uma lista de tecnologias se a operação for bem-sucedida. Caso contrário, retorna um erro interno do servidor.
    /// </remarks>
    /// <response code="200">Retorna a lista de tecnologias. A lista pode estar vazia se não houver tecnologias registradas.</response>
    /// <response code="500">Retorna um detalhe de problema se ocorrer um erro ao acessar o banco de dados ou durante a execução da consulta.</response>
    /// <returns>Uma ação que resulta em uma resposta contendo uma lista de objetos de tecnologia ou um erro.</returns>
    [ProducesResponseType(typeof(List<TechnologyDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("All")]
    public async Task<ActionResult<IEnumerable<TechnologyDTO>>> GetList()
    {
        var getList = await _serviceTechnology.SelectAllAsync();
        return Ok(getList);
    }

    /// <summary>
    /// Obtém uma tecnologia específica pelo seu ID.
    /// </summary>
    /// <remarks>
    /// Este endpoint realiza uma busca no banco de dados para encontrar uma tecnologia com o ID especificado.
    /// Se uma tecnologia correspondente for encontrada, ela será retornada.
    /// Caso contrário, se não houver uma tecnologia com o ID fornecido, um erro 404 será retornado.
    /// </remarks>
    /// <param name="id">O ID da tecnologia a ser recuperada.</param>
    /// <response code="200">Retorna a tecnologia encontrada.</response>
    /// <response code="404">Retorna um detalhe de problema se nenhuma tecnologia for encontrada com o ID especificado.</response>
    /// <returns>Uma ação que resulta em uma resposta contendo um objeto de tecnologia ou um erro.</returns>
    [ProducesResponseType(typeof(List<TechnologyDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TechnologyDTO>> GetById(int id)
    {
        var getCompanyById = await _serviceTechnology.SelectByIdAsync(id);

        return Ok(getCompanyById);
    }

    /// <summary>
    /// Adiciona uma nova tecnologia ao sistema.
    /// </summary>
    /// <remarks>
    /// Este endpoint realiza a inserção de um objeto de tecnologia no banco de dados.
    /// Retorna o objeto inserido com seu identificador único caso a inserção seja bem-sucedida.
    /// </remarks>
    /// <param name="technologyDTO">O objeto de tecnologia que será inserido.</param>
    /// <response code="200">Retorna o objeto de tecnologia recém-criado com seu identificador único.</response>
    /// <response code="400">Se os dados do objeto não estiverem válidos ou a operação de inserção falhar.</response>
    /// <returns>Um resultado de ação com o status apropriado e os dados (se houverem).</returns>
    [ProducesResponseType(typeof(TechnologyDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] TechnologyDTO technologyDTO)
    {
        if (!ModelState.IsValid) return CustomResponse(modelState: ModelState, title: "Requisição Inválida", method: "POST", url: "api/Technologys", statusCode: 400);

        await _serviceTechnology.InsertAsync(technologyDTO);

        if (!ValidOperation()) return CustomResponse(title: "Requisição Inválida", method: "POST", url: "api/Technologys", statusCode: 400);

        return CreatedAtAction(nameof(Post), new { technologyDTO.TechnologyId }, technologyDTO);
    }

    /// <summary>
    /// Atualiza uma tecnologia existente com base no ID fornecido.
    /// </summary>
    /// <remarks>
    /// Este endpoint realiza a atualização de um registro específico de tecnologia. 
    /// O ID da tecnologia deve ser fornecido na URL e o objeto atualizado no corpo da requisição.
    /// Se o ID na URL não corresponder ao ID no corpo do objeto, a solicitação será rejeitada.
    /// Se a tecnologia especificada não for encontrada, um erro 404 será retornado.
    /// Se a atualização for bem-sucedida, retorna o objeto de tecnologia atualizado.
    /// Se ocorrer um erro durante a atualização, um erro 400 será retornado.
    /// </remarks>
    /// <param name="id">O ID da tecnologia a ser atualizada.</param>
    /// <param name="technologyDTO">O objeto de tecnologia atualizado.</param>
    /// <response code="200">Retorna a tecnologia atualizada.</response>
    /// <response code="400">Retorna um detalhe de problema se houver inconsistência de dados ou falha na validação.</response>
    /// <response code="404">Retorna um detalhe de problema se a tecnologia não for encontrada.</response>
    /// <returns>Uma ação que resulta em uma resposta contendo um objeto de tecnologia atualizado ou um erro.</returns>
    [ProducesResponseType(typeof(TechnologyDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<TechnologyDTO>> Put(int id, [FromBody] TechnologyDTO technologyDTO)
    {
        if (id != technologyDTO.TechnologyId)
        {
            NotificationError("Ocorreu um erro durante a tentativa de atualização do registro: o ID fornecido na solicitação não corresponde ao ID do objeto em questão");
            return CustomResponse(title: "Requisição Invalida", method: "PUT", url: "api/Collaborators", statusCode: 400);
        }

        var getById = await _serviceTechnology.SelectByIdAsync(id);

        if (getById is null)
        {
            return NotFound(new
            {
                Instance = $"api/Collaborators/{id}",
                status = 404,
                error = "Chamada não localizada",
                message = "Ocorreu um erro ao tentar localizar sua chamada",
                Method = "PUT",
            });
        }

        await _serviceTechnology.UpdateAsync(technologyDTO);

        if (!ValidOperation()) return CustomResponse(title: "Requisição Invalida", method: "PUT", url: "api/Collaborators", statusCode: 400);

        return Ok(technologyDTO);
    }

    /// <summary>
    /// Exclui uma tecnologia específica pelo seu ID.
    /// </summary>
    /// <remarks>
    /// Este endpoint remove uma tecnologia existente do banco de dados com base no ID fornecido.
    /// Se a tecnologia não for encontrada, um erro 404 será retornado.
    /// Se a tecnologia for encontrada mas ocorrer um erro durante o processo de exclusão, um erro 400 será retornado.
    /// Se a exclusão for bem-sucedida, a resposta será 204 No Content, indicando que a tecnologia foi removida com sucesso.
    /// </remarks>
    /// <param name="id">O ID da tecnologia a ser excluída.</param>
    /// <response code="204">Retorna um sucesso sem conteúdo se a tecnologia foi excluída com sucesso.</response>
    /// <response code="400">Retorna um detalhe de problema se houver um erro ao processar a solicitação de exclusão.</response>
    /// <response code="404">Retorna um detalhe de problema se nenhuma tecnologia for encontrada com o ID especificado.</response>
    /// <returns>Uma resposta de ação que pode ser No Content (204), Not Found (404), ou Bad Request (400).</returns>
    [ProducesResponseType(typeof(TechnologyDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var getById = await _serviceTechnology.SelectByIdAsync(id);

        if (getById is null)
        {
            return NotFound(new
            {
                Instance = $"api/Technologys/{id}",
                status = 404,
                error = "Chamada não encontrada",
                message = "Ocorreu um erro ao tentar localizar sua chamada.",
                Method = "DELETE",
            });
        }

        await _serviceTechnology.DeleteAsync(id);

        if (!ValidOperation()) return CustomResponse(title: "Requisição Inválida", method: "DELTE", url: "api/Collaborators", statusCode: 400);

        return NoContent();
    }
}
