using System;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using Infotecs.MiniJournal.Application.Interfaces;
using Infotecs.MiniJournal.Application.ViewModels;
using RabbitMQ.Client;

namespace Infotecs.MiniJournal.Application
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private readonly IContainer container;

        public App()
        {
            var containerBuilder = new AutofacContainerBuilder();
            container = containerBuilder.Build();

            DispatcherUnhandledException += OnDispatcherUnhandledException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var connectionFactory = container.Resolve<IConnectionFactory>();
            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var logger = container.Resolve<ILogger>();
                logger.LogEnvironmentInfo();

                var notificationService = container.Resolve<INotificationService>(new TypedParameter(typeof(IModel), channel));
                notificationService.Initialize();

                var window = new Views.MainWindow
                {
                    DataContext = container.Resolve<ArticlesViewModel>()
                };

                window.ShowDialog();
            }
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            bool errorLogged = false;
            try
            {
                var logger = container.Resolve<ILogger>();
                logger.LogError(args.Exception);
                errorLogged = true;
            }
            catch (Exception)
            {
                // ignored
            }
            args.Handled = errorLogged;
        }
    }
}
