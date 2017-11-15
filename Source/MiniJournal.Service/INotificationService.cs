using System;
using Infotecs.MiniJournal.Contracts.Notification;

namespace Infotecs.MiniJournal.Service
{
    public interface INotificationService
    {
        void Initialize();
        void Notify(NotificationMessage message);
    }
}