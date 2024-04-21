using Leanwork.Rh.Application.DTO.Interview;
using Leanwork.Rh.Application.DTO.JobInterviewWeight;
using Leanwork.Rh.Application.Interface;
using Leanwork.Rh.Application.Service;
using Leanwork.Rh.Domain.Interface;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

namespace Leanwork.Rh.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class JobsInterviewsWeightsController : MainController
    {
        IServiceJobInterviewWeight _iServiceJobInterviewWeight;

        public JobsInterviewsWeightsController(IServiceJobInterviewWeight iServiceJobInterviewWeight, INotificationError notificationError) : base(notificationError)
        {
            _iServiceJobInterviewWeight = iServiceJobInterviewWeight;
        }

        /// <summary>
        /// Cria um novo peso de entrevista de vaga.
        /// </summary>
        /// <remarks>
        /// Este endpoint recebe um DTO de peso de entrevista de vaga e insere no sistema.
        /// Validações são realizadas para garantir que o DTO está correto. Se a validação falhar ou a inserção não for bem-sucedida,
        /// uma mensagem de erro é retornada.
        /// </remarks>
        /// <param name="jobInterviewWeightDTO">O DTO do peso de entrevista de vaga a ser inserido.</param>
        /// <returns>Um status 201 Created e o DTO inserido em caso de sucesso, ou um erro 400 em caso de falha na validação ou inserção.</returns>
        /// <response code="201">Retorna o DTO do peso de entrevista de vaga criado.</response>
        /// <response code="400">Retorna um erro se a validação ou inserção falharem.</response>
        [ProducesResponseType(typeof(JobInterviewWeightDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] JobInterviewWeightDTO jobInterviewWeightDTO)
        {
            if (!ModelState.IsValid) return CustomResponse(modelState: ModelState, title: "Requisição Inválida", method: "POST", url: $"api/{nameof(JobsInterviewsWeightsController)}", statusCode: 400);

            await _iServiceJobInterviewWeight.Insert(jobInterviewWeightDTO);

            if (!ValidOperation()) return CustomResponse(title: "Requisição Inválida", method: "POST", url: $"api/{nameof(JobsInterviewsWeightsController)}", statusCode: 400);

            return CreatedAtAction(nameof(Post), new { jobInterviewWeightDTO.WeightInterviewVacancyId }, jobInterviewWeightDTO);
        }

        /// <summary>
        /// Deleta uma entrada de peso de entrevista de vaga pelo ID.
        /// </summary>
        /// <param name="id">O identificador do peso de entrevista de vaga a ser deletado.</param>
        /// <returns>Retorna NoContent se o peso de entrevista de vaga for deletado com sucesso; caso contrário, retorna uma resposta de erro.</returns>
        /// <remarks>
        /// Este endpoint tenta deletar uma entrada específica de peso de entrevista de vaga usando o ID fornecido. Se a deleção for bem-sucedida, retorna um status NoContent.
        /// Se a deleção falhar devido a uma validação ou se a entidade não for encontrada, retorna uma mensagem de erro apropriada.
        /// </remarks>
        /// <response code="204">Se o peso de entrevista de vaga for deletado com sucesso, nenhum conteúdo será retornado.</response>
        /// <response code="400">Se houver falha de validação ou a operação não puder ser completada, uma mensagem de erro é retornada.</response>
        /// <response code="404">Se nenhum peso de entrevista de vaga com o ID fornecido for encontrado, um erro de não encontrado é retornado.</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Deleta uma entrada de peso de entrevista de vaga", Description = "Deleta um peso de entrevista de vaga pelo ID. Retorna NoContent em caso de sucesso ou um erro em caso de falha.")]
        public async Task<ActionResult> Delete(int id)
        {
            await _iServiceJobInterviewWeight.Delete(id);

            if (!ValidOperation()) return CustomResponse(title: "Requisição Inválida", method: "DELTE", url: $"api/{nameof(InterviewsController)}", statusCode: 400);

            return NoContent();
        }
    }
}
