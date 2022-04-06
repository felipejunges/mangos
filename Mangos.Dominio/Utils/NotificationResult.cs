using System.Collections.Generic;
using System.Linq;

namespace Mangos.Dominio.Utils
{
    public class NotificationResult
    {
        public NotificationResult()
        {
            Notifications = new List<Notification>();
        }

        public NotificationResult(string message)
        {
            Notifications = new List<Notification>()
            {
                new Notification(message)
            };
        }

        public List<Notification> Notifications { get; private set; }

        public bool IsValid => !Notifications.Any();

        public string FirstMessage => Notifications.Any() ? Notifications.First().Message : string.Empty;

        public void AddNotification(string message)
        {
            Notifications.Add(new Notification(message));
        }

        public void AddNotifications(NotificationResult notificationResult)
        {
            Notifications.AddRange(notificationResult.Notifications);
        }
    }

    public class NotificationResult<T> : NotificationResult
    {
        public bool HasData => Data is not null;

        public T? Data { get; set; }
    }

    public class Notification
    {
        public Notification(string message)
        {
            this.Message = message;
        }

        public string Message { get; private set; }
    }
}