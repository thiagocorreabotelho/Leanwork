using Leanwork.Rh.Domain.Notification;

namespace Leanwork.Rh.Domain.Interface;

public interface INotificationError
{
    void Handle(NotificationError notificationError);
    List<NotificationError> GetNotification();
    bool IsNotification();
}