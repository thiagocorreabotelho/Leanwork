using Leanwork.Rh.API.Controllers;
using Leanwork.Rh.Application;
using Leanwork.Rh.Domain;
using Leanwork.Rh.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Leanwork.Rh.API;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
public class GendersController : MainController
{
    private IServiceGender _serviceGender;

    public GendersController(IServiceGender serviceGender, INotificationError notificationError) : base(notificationError)
    {
        _serviceGender = serviceGender;
    }

    /// <summary>
    /// Obtém uma lista de todos os gêneros registrados.
    /// </summary>
    /// <returns>
    /// Uma ação que resulta em uma resposta HTTP que contém uma lista de <see cref="GenderDTO"/>.
    /// Retorna status 200 (OK) com a lista de gêneros se a operação for bem-sucedida ou status 500 (Erro Interno do Servidor)
    /// se ocorrer um erro durante a obtenção dos dados.
    /// </returns>
    /// <remarks>
    /// Este método HTTP GET é acessado pela rota 'api/[controller]/All' e é responsável por solicitar ao serviço '_serviceGender'
    /// a recuperação de todos os gêneros disponíveis. O método 'SelectAllAsync' do serviço é utilizado para obter os dados,
    /// que são então retornados ao cliente em formato DTO, adequado para transferência de dados.
    /// As respostas para os diferentes cenários são tratadas de acordo com os atributos 'ProducesResponseType', garantindo que os 
    /// consumidores da API recebam informações claras e precisas sobre o resultado da chamada.
    /// </remarks>
    [ProducesResponseType(typeof(List<GenderDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("All")]
    public async Task<ActionResult<IEnumerable<GenderDTO>>> GetList()
    {
        var getList = await _serviceGender.SelectAllAsync();
        return Ok(getList);
    }

    /// <summary>
    /// Obtém um gênero específico pelo seu identificador.
    /// </summary>
    /// <param name="id">O identificador do gênero a ser obtido.</param>
    /// <returns>
    /// Uma ação que resulta em uma resposta HTTP contendo um objeto <see cref="GenderDTO"/>.
    /// Retorna status 200 (OK) com o gênero se encontrado, ou status 404 (Não Encontrado)
    /// se nenhum gênero for encontrado com o identificador fornecido.
    /// </returns>
    /// <remarks>
    /// Este método HTTP GET é acessado pela rota 'api/[controller]/{id}' e é responsável por solicitar ao serviço
    /// '_serviceGender' a recuperação de um gênero específico através de seu ID. O método 'SelectByIdAsync' do serviço é utilizado
    /// para obter o dado, que é então retornado ao cliente em formato DTO, adequado para transferência de dados.
    /// As respostas para os diferentes cenários são tratadas de acordo com os atributos 'ProducesResponseType', garantindo que os
    /// consumidores da API recebam informações claras e precisas sobre o resultado da chamada.
    /// </remarks>
    [ProducesResponseType(typeof(List<GenderDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GenderDTO>> GetById(int id)
    {
        var getCompanyById = await _serviceGender.SelectByIdAsync(id);

        return Ok(getCompanyById);
    }

    /// <summary>
    /// Cria um novo registro de gênero no sistema.
    /// </summary>
    /// <param name="genderDTO">O objeto DTO <see cref="GenderDTO"/> que contém os dados do gênero a ser criado.</param>
    /// <returns>
    /// Uma ação que resulta em uma resposta HTTP. Retorna status 201 (Criado) com os detalhes do gênero criado,
    /// ou status 400 (Requisição Inválida) se os dados fornecidos forem inválidos ou se a operação falhar.
    /// </returns>
    /// <remarks>
    /// Este método HTTP POST é acessado pela rota 'api/Genders' e é responsável por receber os dados de um gênero
    /// via corpo da requisição, validando e inserindo no sistema através do serviço '_serviceGender'.
    /// A validação do modelo é verificada pelo 'ModelState.IsValid'. Se o modelo for inválido, uma resposta customizada é retornada.
    /// Após a inserção, é feita uma verificação com 'ValidOperation()' para garantir que não houve erros durante a inserção.
    /// Se a operação for bem-sucedida, a resposta inclui o URI do recurso criado juntamente com os dados do gênero.
    /// Caso contrário, uma resposta de erro é retornada com detalhes específicos do problema.
    /// </remarks>
    [ProducesResponseType(typeof(GenderDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] GenderDTO genderDTO)
    {
        if (!ModelState.IsValid) return CustomResponse(modelState: ModelState, title: "Requisição Inválida", method: "POST", url: "api/Genders", statusCode: 400);

        await _serviceGender.InsertAsync(genderDTO);

        if (!ValidOperation()) return CustomResponse(title: "Requisição Inválida", method: "POST", url: "api/Genders", statusCode: 400);

        return CreatedAtAction(nameof(Post), new { genderDTO.GenderId }, genderDTO);
    }

    /// <summary>
    /// Atualiza os dados de um registro de gênero existente.
    /// </summary>
    /// <param name="id">O identificador do gênero a ser atualizado.</param>
    /// <param name="genderDTO">O objeto DTO <see cref="GenderDTO"/> que contém os dados atualizados do gênero.</param>
    /// <returns>
    /// Uma ação que resulta em uma resposta HTTP. Retorna status 200 (OK) com os dados atualizados do gênero,
    /// status 400 (Requisição Inválida) se ocorrer inconsistência de dados ou erro na operação, ou status 404 (Não Encontrado)
    /// se nenhum gênero for encontrado com o identificador fornecido.
    /// </returns>
    /// <remarks>
    /// Este método HTTP PUT é acessado pela rota 'api/Genders/{id}' e é responsável por atualizar um gênero específico.
    /// Inicialmente, verifica se o ID passado na URL corresponde ao ID no corpo do DTO. Se houver discrepância, retorna um erro 400.
    /// Caso o gênero não seja encontrado, retorna um erro 404. Se essas verificações forem bem-sucedidas, prossegue com a atualização
    /// dos dados do gênero através do serviço '_serviceGender'. A operação de atualização é validada por 'ValidOperation()'.
    /// Se a operação falhar, retorna um erro 400; caso contrário, retorna os dados do gênero atualizado.
    /// </remarks>
    [ProducesResponseType(typeof(GenderDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<GenderDTO>> Put(int id, [FromBody] GenderDTO genderDTO)
    {
        if (id != genderDTO.GenderId)
        {
            NotificationError("Ocorreu um erro durante a tentativa de atualização do registro: o ID fornecido na solicitação não corresponde ao ID do objeto em questão");
            return CustomResponse(title: "Requisição Invalida", method: "PUT", url: $"api/{nameof(GendersController)}", statusCode: 400);
        }

        var getById = await _serviceGender.SelectByIdAsync(id);

        if (getById is null)
        {
            return NotFound(new
            {
                Instance = $"api/{nameof(GendersController)}/{id}",
                status = 404,
                error = "Chamada não localizada",
                message = "Ocorreu um erro ao tentar localizar sua chamada",
                Method = "PUT",
            });
        }

        await _serviceGender.UpdateAsync(genderDTO);

        if (!ValidOperation()) return CustomResponse(title: "Requisição Invalida", method: "PUT", url: $"api/{nameof(GendersController)}", statusCode: 400);

        return Ok(genderDTO);
    }

    /// <summary>
    /// Exclui um registro de gênero existente com base no seu identificador.
    /// </summary>
    /// <param name="id">O identificador do gênero a ser excluído.</param>
    /// <returns>
    /// Uma ação que resulta em uma resposta HTTP. Retorna status 204 (No Content) se o gênero for excluído com sucesso,
    /// status 400 (Requisição Inválida) se a operação falhar após a exclusão, ou status 404 (Não Encontrado) se nenhum gênero
    /// for encontrado com o identificador fornecido.
    /// </returns>
    /// <remarks>
    /// Este método HTTP DELETE é acessado pela rota 'api/Genders/{id}' e é responsável por excluir um gênero específico.
    /// Inicialmente, verifica se o gênero existe chamando o método 'SelectByIdAsync' do serviço '_serviceGender'. Se o gênero
    /// não for encontrado, retorna um erro 404 com detalhes sobre a falha na localização. Se encontrado, prossegue com a exclusão
    /// através do método 'DeleteAsync'. Após a tentativa de exclusão, verifica se a operação foi bem-sucedida com 'ValidOperation()'.
    /// Se houver falhas na operação pós-exclusão, retorna um erro 400; caso contrário, retorna um 204 (No Content) indicando que
    /// o gênero foi excluído sem conteúdo a retornar.
    /// </remarks>
    [ProducesResponseType(typeof(GenderDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var getById = await _serviceGender.SelectByIdAsync(id);

        if (getById is null)
        {
            return NotFound(new
            {
                Instance = $"api/{nameof(GendersController)}/{id}",
                status = 404,
                error = "Chamada não encontrada",
                message = "Ocorreu um erro ao tentar localizar sua chamada.",
                Method = "DELETE",
            });
        }

        await _serviceGender.DeleteAsync(id);

        if (!ValidOperation()) return CustomResponse(title: "Requisição Inválida", method: "DELTE", url: $"api/{nameof(GendersController)}", statusCode: 400);

        return NoContent();
    }
}
