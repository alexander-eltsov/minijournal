using Autofac;
using Infotecs.MiniJournal.Application.Properties;
using Infotecs.MiniJournal.Application.ViewModels;
using Infotecs.MiniJournal.Application.Views;
using Infotecs.MiniJournal.Contracts;
using Nelibur.ServiceModel.Clients;

namespace Infotecs.MiniJournal.Application
{
    public class AutofacContainerBuilder
    {
        public IContainer Build()
        {
            var builder = new ContainerBuilder();

            // system
            builder.Register(context => MiniJournalDependencyResolver.Instance()).As<IDependencyResolver>();
            builder.RegisterType<Logger>().As<ILogger>();
            builder.RegisterType<NotifiactionService>().As<INotificationService>();

            // services
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
