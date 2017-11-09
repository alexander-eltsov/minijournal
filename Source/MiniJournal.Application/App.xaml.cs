using System;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using Infotecs.MiniJournal.Application.ViewModels;

namespace Infotecs.MiniJournal.Application
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private readonly IDependencyResolver resolver;

        public App()
        {
            resolver = new MiniJournalDependencyResolver();
            DispatcherUnhandledException += OnDispatcherUnhandledException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var logger = resolver.Resolve<ILogger>();
            logger.LogEnvironmentInfo();

            var window = new Views.MainWindow
            {
                DataContext = resolver.Resolve<ArticlesViewModel>()
            };
            window.Show();
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            bool errorLogged = false;
            try
            {
                var logger = resolver.Resolve<ILogger>();
                logger.LogError(args.Exception);
                errorLogged = true;
            }
            catch (Exception)
            {
                // ignored
            }

            bool errorNotified = false;
            try
            {
                var notificationService = resolver.Resolve<INotificationService>();
                notificationService.NotifyError(args.Exception.Message);
                errorNotified = true;
            }
            catch (Exception)
            {
                // ignored
            }

            args.Handled = errorLogged || errorNotified;
        }
    }
}
