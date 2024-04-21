using Leanwork.Rh.Domain.Interface;
using Leanwork.Rh.Domain.Notification;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace Leanwork.Rh.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
public class MainController : ControllerBase
{
    #region private fields

    private readonly INotificationError _notificationError;

    #endregion

    #region Constructor

    public MainController(INotificationError notificationError)
    {
        _notificationError = notificationError;
    }

    #endregion

    /// <summary>
    /// Method responsible for checking if you have notification
    /// </summary>
    /// <returns>Return true for not notification and false for then notification</returns>
    protected bool ValidOperation()
    {
        return !_notificationError.IsNotification();
    }

    /// <summary>
    /// Method responsible for retunr response customized
    /// </summary>
    /// <param name="result">expected outcome</param>
    /// <param name="title">Title Error</param>
    /// <param name="url">Url error</param>
    /// <param name="method">Type request</param>
    protected ActionResult CustomResponse(object result = null, string? title = null, string? method = null, string? url = null, int? statusCode = 0)
    {
        if (ValidOperation())
        {
            return Ok(new
            {
                success = true,
                data = result
            });
        }
        return BadRequest(new
        {
            Metgod = method,
            type = url,
            Title = title,
            status = statusCode,
            success = false,
            errors = _notificationError.GetNotification().Select(n => n.Messege)
        });
    }

    /// <summary>
    /// Method responsible for return i custom response
    /// </summary>
    /// <param name="modelState"></param>
    /// <param name="title"></param>
    /// <param name="method"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    protected ActionResult CustomResponse(ModelStateDictionary modelState, string? title = null, string? method = null, string? url = null, int? statusCode = 0)
    {
        if (!modelState.IsValid) NotificationErrorModelInvalida(modelState);
        return CustomResponse(title: title, method: method, url: url, statusCode: statusCode);
    }

    /// <summary>
    /// Method responsible for validation in model.
    /// </summary>
    /// <param name="modelState">Object model</param>
    protected void NotificationErrorModelInvalida(ModelStateDictionary modelState)
    {
        var erros = modelState.Values.SelectMany(e => e.Errors);
        foreach (var erro in erros)
        {
            var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
            NotificationError(errorMsg);
        }
    }

    /// <summary>
    /// Method responsible for checking is notification.
    /// </summary>
    /// <param name="mensagem"></param>
    protected void NotificationError(string mensagem)
    {
        _notificationError.Handle(new NotificationError(mensagem));
    }
}
