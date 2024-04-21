using Leanwork.Rh.API.Controllers;
using Leanwork.Rh.Application;
using Leanwork.Rh.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Leanwork.Rh.API;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
public class CompaniesController : MainController
{
    private readonly IServiceCompany _serviceCompany;

    public CompaniesController(IServiceCompany serviceCompany, INotificationError notificationError) : base(notificationError)
    {
        _serviceCompany = serviceCompany;
    }

    /// <summary>
    /// Obtém uma lista de todas as empresas registradas no sistema.
    /// </summary>
    /// <remarks>
    /// Este endpoint realiza uma consulta para obter todos os registros de empresas existentes no banco de dados e os retorna como uma lista de objetos DTO (Data Transfer Object). 
    /// É útil para operações que necessitam visualizar uma lista completa das empresas, por exemplo, para seleções em interfaces de usuário ou relatórios.
    /// </remarks>
    /// <response code="200">Retorna uma lista de empresas. A lista pode estar vazia se não houver empresas registradas.</response>
    /// <response code="500">Retorna um detalhe de problema se ocorrer um erro no servidor ao processar a requisição.</response>
    /// <returns>Uma ação que resulta em uma resposta contendo uma lista de objetos de empresa (CompanyDTO) ou um erro.</returns>
    [ProducesResponseType(typeof(List<CompanyDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("All")]
    public async Task<ActionResult<IEnumerable<CompanyDTO>>> GetList()
    {
        var getList = await _serviceCompany.SelectAllAsync();
        return Ok(getList);
    }

    /// <summary>
    /// Obtém uma empresa específica pelo seu ID.
    /// </summary>
    /// <remarks>
    /// Este endpoint busca uma empresa específica no banco de dados usando o ID fornecido como parâmetro da URL. 
    /// A operação retorna um objeto `CompanyDTO` que representa a empresa, caso seja encontrada. 
    /// Se não houver uma empresa correspondente ao ID fornecido, a resposta será uma mensagem de não encontrado (404).
    /// </remarks>
    /// <param name="id">O ID da empresa a ser recuperada.</param>
    /// <response code="200">Retorna o objeto `CompanyDTO` da empresa encontrada.</response>
    /// <response code="404">Retorna um detalhe de problema se nenhuma empresa for encontrada com o ID especificado.</response>
    /// <returns>Uma ação que resulta em uma resposta contendo um objeto `CompanyDTO` ou um erro.</returns>
    [ProducesResponseType(typeof(List<CompanyDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CompanyDTO>> GetById(int id)
    {
        var getCompanyById = await _serviceCompany.SelectByIdAsync(id);

        return Ok(getCompanyById);
    }

    /// <summary>
    /// Cria uma nova empresa no sistema.
    /// </summary>
    /// <remarks>
    /// Este endpoint recebe um objeto `CompanyDTO` no corpo da requisição e tenta criar uma nova empresa no banco de dados.
    /// Antes de inserir, valida se o modelo de dados recebido é válido. Se houver algum problema com os dados fornecidos,
    /// uma resposta de erro 400 (Bad Request) é retornada, com detalhes do problema.
    /// Se a inserção for bem-sucedida, retorna o objeto `CompanyDTO` criado com o ID atribuído, utilizando o status 201 (Created).
    /// Se a inserção falhar devido a uma operação inválida detectada pelo serviço, também retorna uma resposta de erro 400.
    /// </remarks>
    /// <param name="companyDTO">O objeto `CompanyDTO` com os dados da empresa a ser criada.</param>
    /// <response code="201">Retorna o objeto `CompanyDTO` criado com informações adicionais como ID.</response>
    /// <response code="400">Retorna um detalhe de problema se os dados do objeto não forem válidos ou a operação falhar.</response>
    /// <returns>Uma resposta de ação que pode ser um `CreatedAtAction` com o `CompanyDTO` ou um `BadRequest` com detalhes do problema.</returns>
    [ProducesResponseType(typeof(CompanyDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CompanyDTO companyDTO)
    {
        if (!ModelState.IsValid) return CustomResponse(modelState: ModelState, title: "Requisição Inválida", method: "POST", url: "api/Companies", statusCode: 400);

        await _serviceCompany.InsertAsync(companyDTO);

        if (!ValidOperation()) return CustomResponse(title: "Requisição Inválida", method: "POST", url: "api/Companies", statusCode: 400);

        return CreatedAtAction(nameof(Post), new { companyDTO.CompanyId }, companyDTO);
    }

    /// <summary>
    /// Atualiza uma empresa existente com base no ID fornecido.
    /// </summary>
    /// <remarks>
    /// Este endpoint recebe um ID e um objeto `CompanyDTO` no corpo da requisição para atualizar a empresa correspondente.
    /// A operação primeiro verifica se o ID fornecido na URL corresponde ao ID no `CompanyDTO`. Se não corresponder,
    /// uma resposta de erro 400 (Bad Request) é retornada com uma mensagem de erro.
    /// Em seguida, verifica se a empresa com o ID fornecido existe. Se não existir, retorna uma resposta 404 (Not Found).
    /// Se a empresa existir, tenta realizar a atualização. Se a operação de atualização falhar devido a uma validação de negócio,
    /// uma resposta de erro 400 é retornada. Se a atualização for bem-sucedida, retorna o objeto `CompanyDTO` atualizado com status 200 (OK).
    /// </remarks>
    /// <param name="id">O ID da empresa que está sendo atualizada.</param>
    /// <param name="companyDTO">O objeto `CompanyDTO` com os dados atualizados da empresa.</param>
    /// <response code="200">Retorna o objeto `CompanyDTO` atualizado.</response>
    /// <response code="400">Retorna um detalhe de problema se houver inconsistência entre os IDs, dados inválidos ou falha na validação.</response>
    /// <response code="404">Retorna um detalhe de problema se nenhuma empresa for encontrada com o ID especificado.</response>
    /// <returns>Uma resposta de ação que pode ser um `Ok` com o `CompanyDTO` atualizado, um `BadRequest` ou um `NotFound`.</returns>
    [ProducesResponseType(typeof(CompanyDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<CompanyDTO>> Put(int id, [FromBody] CompanyDTO companyDTO)
    {
        if (id != companyDTO.CompanyId)
        {
            NotificationError("Ocorreu um erro durante a tentativa de atualização do registro: o ID fornecido na solicitação não corresponde ao ID do objeto em questão");
            return CustomResponse(title: "Requisição Invalida", method: "PUT", url: "api/Collaborators", statusCode: 400);
        }

        var getById = await _serviceCompany.SelectByIdAsync(id);

        if (getById is null)
        {
            return NotFound(new
            {
                Instance = $"api/{nameof(CompaniesController)}/{id}",
                status = 404,
                error = "Chamada não localizada",
                message = "Ocorreu um erro ao tentar localizar sua chamada",
                Method = "PUT",
            });
        }

        await _serviceCompany.UpdateAsync(companyDTO);

        if (!ValidOperation()) return CustomResponse(title: "Requisição Invalida", method: "PUT", url: $"api/{nameof(CompaniesController)}", statusCode: 400);

        return Ok(companyDTO);
    }

    /// <summary>
    /// Exclui uma empresa existente com base no ID fornecido.
    /// </summary>
    /// <remarks>
    /// Este endpoint recebe um ID como parâmetro de rota e tenta excluir a empresa correspondente no banco de dados.
    /// Inicialmente, verifica se a empresa com o ID especificado existe. Se não for encontrada, retorna uma resposta 404 (Not Found) com detalhes do erro.
    /// Se a empresa existir, procede com a tentativa de exclusão. Se a operação de exclusão falhar devido a uma verificação de negócio (por exemplo, verificações de validação),
    /// retorna uma resposta de erro 400 (Bad Request). Se a exclusão for bem-sucedida, retorna uma resposta 204 (No Content), indicando que a operação foi completada sem retornar dados.
    /// </remarks>
    /// <param name="id">O ID da empresa a ser excluída.</param>
    /// <response code="204">Indica que a empresa foi excluída com sucesso, sem retornar nenhum conteúdo.</response>
    /// <response code="400">Retorna um detalhe de problema se houver um erro durante a exclusão, como uma falha de validação.</response>
    /// <response code="404">Retorna um detalhe de problema se nenhuma empresa for encontrada com o ID especificado.</response>
    /// <returns>Uma resposta de ação que pode ser um `NoContent`, um `BadRequest` ou um `NotFound`.</returns>
    [ProducesResponseType(typeof(CompanyDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var getById = await _serviceCompany.SelectByIdAsync(id);

        if (getById is null)
        {
            return NotFound(new
            {
                Instance = $"api/{nameof(CompaniesController)}/{id}",
                status = 404,
                error = "Chamada não encontrada",
                message = "Ocorreu um erro ao tentar localizar sua chamada.",
                Method = "DELETE",
            });
        }

        await _serviceCompany.DeleteAsync(id);

        if (!ValidOperation()) return CustomResponse(title: "Requisição Inválida", method: "DELTE", url: $"api/{nameof(CompaniesController)}", statusCode: 400);

        return NoContent();
    }
}
