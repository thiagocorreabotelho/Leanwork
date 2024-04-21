using FluentValidation;
using FluentValidation.Results;

using Leanwork.Rh.Domain.Interface;
using Leanwork.Rh.Domain.Notification;

namespace Leanwork.Rh.Application.Service;

public class ServiceBase
{
    private readonly INotificationError _notificationError;

    protected ServiceBase(INotificationError notificationError)
    {
        _notificationError = notificationError;
    }

    /// <summary>
    /// Notifica os erros de validação contidos no objeto ValidationResult.
    /// </summary>
    /// <param name="validationResult">O objeto ValidationResult contendo os erros de validação.</param>
    protected void Notify(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
        {
            Notify(error.ErrorMessage);
        }
    }

    /// <summary>
    /// Notifica um erro utilizando o serviço de tratamento de erros especificado.
    /// </summary>
    /// <param name="message">A mensagem de erro a ser notificada.</param>
    protected void Notify(string message)
    {
        _notificationError.Handle(new NotificationError(message));
    }

    /// <summary>
    /// Executa a validação de uma entidade usando um validador específico.
    /// </summary>
    /// <typeparam name="TV">O tipo do validador.</typeparam>
    /// <typeparam name="TE">O tipo da entidade a ser validada.</typeparam>
    /// <param name="validation">O validador a ser utilizado para validar a entidade.</param>
    /// <param name="entity">A entidade a ser validada.</param>
    /// <returns>
    /// True se a entidade passar na validação; False caso contrário.
    /// </returns>
    protected bool RunValidation<TV, TE>(TV validation, TE entity) where TV : AbstractValidator<TE>
    {
        var validator = validation.Validate(entity);

        if (validator.IsValid) return true;

        Notify(validator);

        return false;
    }

    
}
