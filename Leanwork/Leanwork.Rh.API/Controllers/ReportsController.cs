using Leanwork.Rh.Application.DTO.ReportCandidate;
using Leanwork.Rh.Application.DTO.Technology;
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
    public class ReportsController : MainController
    {
        readonly IServiceReportCandidate _iServiceReportCandidate;
        public ReportsController(IServiceReportCandidate iServiceReportCandidate, INotificationError notificationError) : base(notificationError)
        {
            _iServiceReportCandidate = iServiceReportCandidate;
        }

        /// <summary>
        /// Recupera todos os dados dos candidatos para relatório, formatados como DTOs.
        /// </summary>
        /// <remarks>
        /// Este endpoint fornece uma lista completa de todos os candidatos, contendo informações detalhadas para relatórios, formatados como DTOs.
        /// </remarks>
        /// <response code="200">Lista de DTOs de candidatos para relatório recuperada com sucesso.</response>
        /// <response code="500">Ocorreu um erro no servidor ao processar a solicitação.</response>
        [HttpGet("All")]
        [ProducesResponseType(typeof(List<ReportCandidateDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Recupera todos os candidatos para relatório", Description = "Obtém uma lista completa de todos os candidatos para relatório, formatados como DTOs.")]
        public async Task<ActionResult<IEnumerable<ReportCandidateDTO>>> GetList()
        {
            var getList = await _iServiceReportCandidate.SelectReportCandidate();
            return Ok(getList);
        }
    }
}
