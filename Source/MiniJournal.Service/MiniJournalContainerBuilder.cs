using System;
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
            builder.RegisterType<ArticleRepository>().As<IArticleRepository>();

            builder.RegisterType<ArticleService>().As<IArticleService>();

            return builder.Build();
        }
    }
}
