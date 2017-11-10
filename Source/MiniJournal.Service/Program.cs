using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using Autofac;
using Infotecs.MiniJournal.Contracts;
using Infotecs.MiniJournal.Service.MessageProcessors;
using Nelibur.ServiceModel.Services;
using Nelibur.ServiceModel.Services.Default;

namespace Infotecs.MiniJournal.Service
{
    public class Program
    {
        private static void ConfigureService(IContainer container)
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

        private static void Main(string[] args)
        {
            var containerBuilder = new MiniJournalContainerBuilder();
            using (var container = containerBuilder.Build())
            {
                ConfigureService(container);

                var host = new WebServiceHost(typeof(JsonServicePerCall));
                try
                {
                    host.Open();

                    Console.WriteLine("Service is running");
                    Console.WriteLine("Press enter to quit...");
                    Console.ReadLine();

                    host.Close();
                }
                catch (CommunicationException cex)
                {
                    Console.WriteLine("An exception occurred: {0}", cex.Message);
                    host.Abort();
                }
            }
        }
    }
}
