using System;
using System.Windows;

namespace Infotecs.MiniJournal.Application
{
    public interface INotificationService
    {
        void NotifyError(string error);
    }

    public class NotifiactionService : INotificationService
    {
        public void NotifyError(string error)
        {
            MessageBox.Show(error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
