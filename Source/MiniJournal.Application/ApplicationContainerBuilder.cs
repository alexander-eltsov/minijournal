using Autofac;
using Infotecs.MiniJournal.Application.ArticleServiceReference;
using Infotecs.MiniJournal.Application.ViewModels;

namespace Infotecs.MiniJournal.Application
{
    public class ApplicationContainerBuilder
    {
        public IContainer Build()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ArticleServiceClient>().As<IArticleService>();

            builder.RegisterType<ArticlesViewModel>().AsSelf();

            return builder.Build();
        }
    }
}
