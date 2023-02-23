using System;
using System.Collections.Generic;
using System.Timers;

namespace SimpleCore.Components.Notifications
{
    public enum NotificationType
    {
        Primary,
        Secondary,
        Success,
        Danger,
        Warning,
        Info
    }

    public class Notification: IDisposable
    {
        private System.Timers.Timer _countdown;

        public event Action<Notification> OnHide;

        public string Message { get; set; }

        public NotificationType Type { get; set; }

        public void StartCountdown(int milliseconds)
        {
            _countdown ??= new System.Timers.Timer(milliseconds);

            if (_countdown.Enabled)
                _countdown.Stop();

            _countdown.Elapsed += Hide;
            _countdown.Start();
        }

        private void Hide(object source, ElapsedEventArgs args)
        {
            OnHide.Invoke(this);
        }

        public void Dispose()
        {
            _countdown.Dispose();
        }
    }
    
    public class NotificationsHub
    {
        public ICollection<Notification> Notifications;
        public event Action OnNotificationsChange;

        public NotificationsHub()
        {
            Notifications = new List<Notification>();
        }

        public void Send(Notification notification)
        {
            notification.OnHide += HideNotification;
            notification.StartCountdown(5000);
            Notifications.Add(notification);
            OnNotificationsChange.Invoke();
        }

        private void HideNotification(Notification notification)
        {
            Notifications.Remove(notification);
            OnNotificationsChange.Invoke();
        }
    }
}
