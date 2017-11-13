using System.Configuration;
using Autofac;
using Infotecs.MiniJournal.Dal;
using Infotecs.MiniJournal.Service.MessageProcessors;

namespace Infotecs.MiniJournal.Service
{
    public class MiniJournalContainerBuilder
    {
        public IContainer Build()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MiniJournalMapper>().As<IMapper>();

            // NHibernate
            builder.Register(context =>
            {
                var connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
                return new ArticleRepository(connectionString);
            }).As<IArticleRepository>();

            // message processors
            builder.RegisterType<ArticleHeaderProcessor>().AsSelf();
            builder.RegisterType<ArticleProcessor>().AsSelf();
            builder.RegisterType<CommentProcessor>().AsSelf();

            return builder.Build();
        }
    }
}
