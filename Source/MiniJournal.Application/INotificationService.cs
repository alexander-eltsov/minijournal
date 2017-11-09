using System;

namespace Infotecs.MiniJournal.Application
{
    public interface INotificationService
    {
        void NotifyError(string error);
    }
}
