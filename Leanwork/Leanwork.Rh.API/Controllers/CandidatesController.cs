using AutoMapper;
using Leanwork.Rh.API.Controllers;
using Leanwork.Rh.Application;
using Leanwork.Rh.Application.DTO.Technology;
using Leanwork.Rh.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Leanwork.Rh.API;

[ApiController]
[Route("api/[controller]")]
public class CandidatesController : MainController
{
    private IServiceCandidate _serviceCandidate;
    private IServiceCandidateTechnologyRel _serviceCandidateTechnologyRel;  
    private IMapper _mapper;

    public CandidatesController(IServiceCandidate serviceCandidate, INotificationError notificationError, IServiceCandidateTechnologyRel serviceCandidateTechnologyRel, IMapper mapper) : base(notificationError)
    {
        _serviceCandidate = serviceCandidate;
        _serviceCandidateTechnologyRel = serviceCandidateTechnologyRel;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtém uma lista de todos os candidatos registrados.
    /// </summary>
    /// <returns>
    /// Uma ação que resulta em uma resposta HTTP que contém uma lista de <see cref="CandidateDTO"/>.
    /// Retorna status 200 (OK) com a lista de candidatos se a operação for bem-sucedida ou status 500 (Erro Interno do Servidor)
    /// se ocorrer um erro durante a obtenção dos dados.
    /// </returns>
    /// <remarks>
    /// Este método HTTP GET é acessado pela rota 'api/[controller]/All' e é responsável por solicitar ao serviço '_serviceCandidate'
    /// a recuperação de todos os candidatos disponíveis. O método 'SelectAllAsync' do serviço é utilizado para obter os dados,
    /// que são então retornados ao cliente em formato DTO, adequado para transferência de dados.
    /// As respostas para os diferentes cenários são tratadas de acordo com os atributos 'ProducesResponseType', garantindo que os 
    /// consumidores da API recebam informações claras e precisas sobre o resultado da chamada.
    /// </remarks>
    [ProducesResponseType(typeof(List<CandidateDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("All")]
    public async Task<ActionResult<IEnumerable<CandidateDTO>>> GetList()
    {
        var getList = await _serviceCandidate.SelectAllAsync();
        return Ok(getList);
    }

    /// <summary>
    /// Obtém um candidato específico pelo seu identificador.
    /// </summary>
    /// <param name="id">O identificador do candidato a ser obtido.</param>
    /// <returns>
    /// Uma ação que resulta em uma resposta HTTP contendo um objeto <see cref="CandidateDTO"/>.
    /// Retorna status 200 (OK) com o candidato se encontrado, ou status 404 (Não Encontrado)
    /// se nenhum candidato for encontrado com o identificador fornecido.
    /// </returns>
    /// <remarks>
    /// Este método HTTP GET é acessado pela rota 'api/[controller]/{id}' e é responsável por solicitar ao serviço
    /// '_serviceCandidate' a recuperação de um candidato específico através de seu ID. O método 'SelectByIdAsync' do serviço é utilizado
    /// para obter o dado, que é então retornado ao cliente em formato DTO, adequado para transferência de dados.
    /// As respostas para os diferentes cenários são tratadas de acordo com os atributos 'ProducesResponseType', garantindo que os
    /// consumidores da API recebam informações claras e precisas sobre o resultado da chamada.
    /// </remarks
    [ProducesResponseType(typeof(List<CandidateDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CandidateDTO>> GetById(int id)
    {
        var getCompanyById = await _serviceCandidate.SelectByIdAsync(id);

        return Ok(getCompanyById);
    }

    /// <summary>
    /// Cria um novo candidato no sistema.
    /// </summary>
    /// <param name="candidateDTO">O objeto DTO <see cref="CandidateDTO"/> que contém os dados do candidato a ser criado.</param>
    /// <returns>
    /// Uma ação que resulta em uma resposta HTTP. Retorna status 201 (Criado) com os detalhes do candidato criado,
    /// ou status 400 (Requisição Inválida) se os dados fornecidos forem inválidos ou se a operação falhar.
    /// </returns>
    /// <remarks>
    /// Este método HTTP POST é acessado pela rota 'api/Candidates' e é responsável por receber os dados de um candidato
    /// via corpo da requisição, validando e inserindo no sistema através do serviço '_serviceCandidate'.
    /// A validação do modelo é verificada pelo 'ModelState.IsValid'. Se o modelo for inválido, uma resposta customizada é retornada.
    /// Após a inserção, é feita uma verificação com 'ValidOperation()' para garantir que não houve erros durante a inserção.
    /// Se a operação for bem-sucedida, a resposta inclui o URI do recurso criado juntamente com os dados do candidato.
    /// Caso contrário, uma resposta de erro é retornada com detalhes específicos do problema.
    /// </remarks>
    [ProducesResponseType(typeof(CandidateDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CandidateDTO candidateDTO)
    {
        if (!ModelState.IsValid) return CustomResponse(modelState: ModelState, title: "Requisição Inválida", method: "POST", url: $"api/Candidates", statusCode: 400);

        await _serviceCandidate.InsertAsync(candidateDTO);

        if (!ValidOperation()) return CustomResponse(title: "Requisição Inválida", method: "POST", url: "api/Candidates", statusCode: 400);

        return CreatedAtAction(nameof(Post), new { candidateDTO.CandidateId }, candidateDTO);
    }

    /// <summary>
    /// Atualiza os dados de um candidato existente.
    /// </summary>
    /// <param name="id">O identificador do candidato a ser atualizado.</param>
    /// <param name="candidateDTO">O objeto DTO <see cref="CandidateDTO"/> que contém os dados atualizados do candidato.</param>
    /// <returns>
    /// Uma ação que resulta em uma resposta HTTP. Retorna status 200 (OK) com os dados atualizados do candidato,
    /// status 400 (Requisição Inválida) se ocorrer inconsistência de dados ou erro na operação, ou status 404 (Não Encontrado)
    /// se nenhum candidato for encontrado com o identificador fornecido.
    /// </returns>
    /// <remarks>
    /// Este método HTTP PUT é acessado pela rota 'api/Candidates/{id}' e é responsável por atualizar um candidato específico.
    /// Inicialmente, verifica se o ID passado na URL corresponde ao ID no corpo do DTO. Se houver discrepância, retorna um erro 400.
    /// Caso o candidato não seja encontrado, retorna um erro 404. Se essas verificações forem bem-sucedidas, prossegue com a atualização
    /// dos dados do candidato através do serviço '_serviceCandidate'. A operação de atualização é validada por 'ValidOperation()'.
    /// Se a operação falhar, retorna um erro 400; caso contrário, retorna os dados do candidato atualizado.
    /// </remarks>
    [ProducesResponseType(typeof(CandidateDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<CandidateDTO>> Put(int id, [FromBody] CandidateDTO candidateDTO)
    {
        if (id != candidateDTO.CandidateId)
        {
            NotificationError("Ocorreu um erro durante a tentativa de atualização do registro: o ID fornecido na solicitação não corresponde ao ID do objeto em questão");
            return CustomResponse(title: "Requisição Invalida", method: "PUT", url: "api/Collaborators", statusCode: 400);
        }

        var getById = await _serviceCandidate.SelectByIdAsync(id);

        if (getById is null)
        {
            return NotFound(new
            {
                Instance = $"api/{nameof(CandidatesController)}/{id}",
                status = 404,
                error = "Chamada não localizada",
                message = "Ocorreu um erro ao tentar localizar sua chamada",
                Method = "PUT",
            });
        }

        await _serviceCandidate.UpdateAsync(candidateDTO);

        if (!ValidOperation()) return CustomResponse(title: "Requisição Invalida", method: "PUT", url: $"api/{nameof(CandidatesController)}", statusCode: 400);

        return Ok(candidateDTO);
    }

    /// <summary>
    /// Exclui um candidato existente com base no seu identificador.
    /// </summary>
    /// <param name="id">O identificador do candidato a ser excluído.</param>
    /// <returns>
    /// Uma ação que resulta em uma resposta HTTP. Retorna status 204 (No Content) se o candidato for excluído com sucesso,
    /// status 400 (Requisição Inválida) se a operação falhar após a exclusão, ou status 404 (Não Encontrado) se nenhum candidato
    /// for encontrado com o identificador fornecido.
    /// </returns>
    /// <remarks>
    /// Este método HTTP DELETE é acessado pela rota 'api/Candidates/{id}' e é responsável por excluir um candidato específico.
    /// Inicialmente, verifica se o candidato existe chamando o método 'SelectByIdAsync' do serviço '_serviceCandidate'. Se o candidato
    /// não for encontrado, retorna um erro 404 com detalhes sobre a falha na localização. Se encontrado, prossegue com a exclusão
    /// através do método 'DeleteAsync'. Após a tentativa de exclusão, verifica se a operação foi bem-sucedida com 'ValidOperation()'.
    /// Se houver falhas na operação pós-exclusão, retorna um erro 400; caso contrário, retorna um 204 (No Content) indicando que
    /// o candidato foi excluído sem conteúdo a retornar.
    /// </remarks>
    [ProducesResponseType(typeof(CandidateDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var getById = await _serviceCandidate.SelectByIdAsync(id);

        if (getById is null)
        {
            return NotFound(new
            {
                Instance = $"api/{nameof(CandidatesController)}/{id}",
                status = 404,
                error = "Chamada não encontrada",
                message = "Ocorreu um erro ao tentar localizar sua chamada.",
                Method = "DELETE",
            });
        }

        await _serviceCandidate.DeleteAsync(id);

        if (!ValidOperation()) return CustomResponse(title: "Requisição Inválida", method: "DELTE", url: $"api/{nameof(CandidatesController)}", statusCode: 400);

        return NoContent();
    }
}
