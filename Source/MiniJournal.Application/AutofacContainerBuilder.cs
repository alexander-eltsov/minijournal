using Autofac;
using Infotecs.MiniJournal.Application.ArticleServiceReference;
using Infotecs.MiniJournal.Application.ViewModels;
using Infotecs.MiniJournal.Application.Views;

namespace Infotecs.MiniJournal.Application
{
    public class AutofacContainerBuilder
    {
        public IContainer Build()
        {
            var builder = new ContainerBuilder();

            builder.Register(context => MiniJournalDependencyResolver.Instance()).As<IDependencyResolver>();

            builder.RegisterType<Logger>().As<ILogger>();
            builder.RegisterType<NotifiactionService>().As<INotificationService>();

            builder.RegisterType<ArticleServiceClient>().As<IArticleService>();

            builder.RegisterType<ArticlesViewModel>().AsSelf();
            builder.RegisterType<AddCommentViewModel>().AsSelf();

            builder.RegisterType<AddCommentView>().AsSelf();
            builder.RegisterType<AddCommentViewDialog>().As<IDialogView>();

            return builder.Build();
        }
    }
}
