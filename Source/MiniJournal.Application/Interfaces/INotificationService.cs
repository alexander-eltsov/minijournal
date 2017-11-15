using System;
using Infotecs.MiniJournal.Contracts.Notification;

namespace Infotecs.MiniJournal.Application.Interfaces
{
    public interface INotificationService
    {
        void Initialize();
        void NotifyError(string error);
        void NotifyError(Exception exception);

        void Subscribe<TMessage>(Action<TMessage> handler) where TMessage : NotificationMessage;
        void Unsubscribe<TMessage>(Action<TMessage> handler) where TMessage : NotificationMessage;

        IObservable<TMessage> GetEvent<TMessage>() where TMessage : NotificationMessage;
    }
}
