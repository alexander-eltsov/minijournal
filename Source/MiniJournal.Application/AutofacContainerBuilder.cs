using Autofac;
using Infotecs.MiniJournal.Application.Interfaces;
using Infotecs.MiniJournal.Application.Properties;
using Infotecs.MiniJournal.Application.ViewModels;
using Infotecs.MiniJournal.Application.Views;
using Infotecs.MiniJournal.Contracts;
using Nelibur.ServiceModel.Clients;
using RabbitMQ.Client;

namespace Infotecs.MiniJournal.Application
{
    public class AutofacContainerBuilder
    {
        public IContainer Build()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Logger>().As<ILogger>();

            // message broker
            builder.Register(context => new ConnectionFactory { HostName = "localhost" })
                .As<IConnectionFactory>();

            // services
            builder.RegisterType<RabbitMqNotificationService>().As<INotificationService>().SingleInstance();
            builder.Register(context =>
            {
                var serviceClient = new JsonServiceClient(Settings.Default.ServiceAddress);
                return new JsonArticleServiceClient(serviceClient);
            }).As<IArticleService>();

            // view models
            builder.RegisterType<ArticlesViewModel>().AsSelf();
            builder.RegisterType<AddCommentViewModel>().AsSelf();

            // views
            builder.RegisterType<AddCommentView>().AsSelf();
            builder.RegisterType<AddCommentViewDialog>().As<IDialogView>();

            return builder.Build();
        }
    }
}
