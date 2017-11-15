using System.Configuration;
using Autofac;
using Infotecs.MiniJournal.Contracts;
using Infotecs.MiniJournal.Dal;
using Infotecs.MiniJournal.Service.MessageProcessors;
using Nelibur.ServiceModel.Services;
using RabbitMQ.Client;

namespace Infotecs.MiniJournal.Service
{
    public class MiniJournalContainerBuilder
    {
        public IContainer Build()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MiniJournalMapper>().As<IMapper>();

            // message broker
            builder.Register(context => new ConnectionFactory { HostName = "localhost" })
                .As<IConnectionFactory>();

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

            // services
            builder.RegisterType<RabbitMqNotificationService>()
                .As<INotificationService>()
                .SingleInstance();

            var container = builder.Build();
            ConfigureNeliburRestService(container);

            return container;
        }

        private static void ConfigureNeliburRestService(IContainer container)
        {
            NeliburRestService.Configure(x =>
            {
                x.Bind<GetArticleHeadersRequest, ArticleHeaderProcessor>(() => container.Resolve<ArticleHeaderProcessor>());

                x.Bind<GetArticleRequest, ArticleProcessor>(() => container.Resolve<ArticleProcessor>());
                x.Bind<CreateArticleRequest, ArticleProcessor>(() => container.Resolve<ArticleProcessor>());
                x.Bind<UpdateArticleRequest, ArticleProcessor>(() => container.Resolve<ArticleProcessor>());
                x.Bind<DeleteArticleRequest, ArticleProcessor>(() => container.Resolve<ArticleProcessor>());

                x.Bind<AddCommentRequest, CommentProcessor>(() => container.Resolve<CommentProcessor>());
                x.Bind<RemoveCommentRequest, CommentProcessor>(() => container.Resolve<CommentProcessor>());
            });
        }
    }
}
