using Leanwork.Rh.Domain.Interface;
using Leanwork.Rh.Domain.Notification;

namespace Leanwork.Rh.Application.Notification
{
    public class NoitificationErro : INotificationError
    {
        private readonly List<NotificationError> _notification;

        public NoitificationErro()
        {
            _notification = new List<NotificationError>();
        }

        /// <summary>
        /// Method responsobe for get norification
        /// </summary>
        /// <returns>Return notification</returns>
        public List<NotificationError> GetNotification()
        {
            return _notification;
        }

        /// <summary>
        /// Method responsible for storing error notification
        /// </summary>
        /// <param name="notificationError">Object single notification of error.</param>
        public void Handle(NotificationError notificationError)
        {
            _notification.Add(notificationError);
        }

        /// <summary>
        /// Method responsible for if there is an error 
        /// </summary>
        /// <returns>Returns true if there is an error and false when there is no error.</returns>
        public bool IsNotification()
        {
            return _notification.Any();
        }
    }
}
