using System;
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

            builder.Register(context =>
            {
                var connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
                return new ConnectionFactory(connectionString);
                
            }).As<IConnectionFactory>();

            builder.RegisterType<ArticleRepository>().As<IArticleRepository>();

            builder.RegisterType<ArticleService>().As<IArticleService>();

            return builder.Build();
        }
    }
}
