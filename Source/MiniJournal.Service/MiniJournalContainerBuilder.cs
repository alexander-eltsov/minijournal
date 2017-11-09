using System.Configuration;
using Autofac;
using Infotecs.MiniJournal.Contracts;
using Infotecs.MiniJournal.Dal;

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

            builder.RegisterType<ArticleService>().As<IArticleService>();

            return builder.Build();
        }
    }
}
