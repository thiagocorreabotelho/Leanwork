using Leanwork.Rh.API.Controllers;
using Leanwork.Rh.Application;
using Leanwork.Rh.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Leanwork.Rh.API;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]

public class AddressController : MainController
{
    private IServiceAddress _serviceAddress;

    public AddressController(IServiceAddress serviceAddress, INotificationError notificationError) : base(notificationError)
    {
        _serviceAddress = serviceAddress;
    }

    /// <summary>
    /// Exclui um endereço específico com base no ID fornecido.
    /// </summary>
    /// <param name="id">O identificador único do endereço a ser excluído.</param>
    /// <response code="204">Retorna NoContent se o endereço for excluído com sucesso.</response>
    /// <response code="404">Retorna NotFound se nenhum endereço com o ID especificado for encontrado.</response>
    /// <response code="400">Retorna BadRequest se a operação de exclusão for inválida.</response>
    [ProducesResponseType(typeof(AddressDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var getById = await _serviceAddress.SelectByIdAsync(id);

        if (getById is null)
        {
            return NotFound(new
            {
                Instance = $"api/{nameof(AddressController)}/{id}",
                status = 404,
                error = "Chamada não encontrada",
                message = "Ocorreu um erro ao tentar localizar sua chamada.",
                Method = "DELETE",
            });
        }

        await _serviceAddress.DeleteAsync(id);

        if (!ValidOperation()) return CustomResponse(title: "Requisição Inválida", method: "DELTE", url: $"api/{nameof(AddressController)}", statusCode: 400);

        return NoContent();
    }
}
