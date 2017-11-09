using System;
using System.Windows;

namespace Infotecs.MiniJournal.Application
{
    public class NotifiactionService : INotificationService
    {
        public void NotifyError(string error)
        {
            MessageBox.Show(error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}