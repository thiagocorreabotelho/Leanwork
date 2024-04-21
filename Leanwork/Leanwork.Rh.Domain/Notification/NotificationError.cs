namespace Leanwork.Rh.Domain.Notification;

public class NotificationError
{
    public NotificationError(string message)
    {
        Messege = message;
    }

    public string Messege { get; set; }
}
