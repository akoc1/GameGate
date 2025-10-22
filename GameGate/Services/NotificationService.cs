using GameGate.Models;
using System;

namespace GameGate.Services
{
    public class NotificationService
    {
        private static NotificationService _instance;
        public static NotificationService Instance => _instance ??= new NotificationService();

        public event Action<NotificationMessage> OnNotify;
        public event Action OnHide;

        public void ShowNotification(string message, NotificationType notiType)
        {
            OnNotify?.Invoke(new NotificationMessage
            {
                Message = message,
                NotificationType = notiType
            });
        }

        public void ShowNotification(string message)
        {
            ShowNotification(message, NotificationType.Info);
        }

        public void HideNotification()
        {
            OnHide?.Invoke();
        }
    }
}
