using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Autofac;
using Autofac.Integration.Wcf;
using Infortecs.MiniJournal.Service;
using Infotecs.MiniJournal.Contracts;

namespace MiniJournal.Service
{
    public class Program
    {
        static void Main(string[] args)
        {
            var containerBuilder = new MiniJournalContainerBuilder();
            using (var container = containerBuilder.Build())
            {
                //RunServiceHost(container);

                Uri address = new Uri("http://localhost:8082/article");
                ServiceHost host = new ServiceHost(typeof(ArticleService), address);
                //host.AddServiceEndpoint(typeof(IEchoService), new BasicHttpBinding(), string.Empty);

                // Here's the important part - attaching the DI behavior to the service host
                // and passing in the container.
                host.AddDependencyInjectionBehavior<IArticleService>(container);

                host.Description.Behaviors.Add(new ServiceMetadataBehavior { HttpGetEnabled = true, HttpGetUrl = address });
                host.Open();

                Console.WriteLine("The host has been opened.");
                Console.ReadLine();

                host.Close();
                Environment.Exit(0);
            }
        }

        //private static void RunServiceHost(IContainer container)
        //{
        //    Uri baseAddress = new Uri("http://localhost:8082/article");

        //    // Create the ServiceHost.
        //    using (ServiceHost host = new ServiceHost(typeof(ArticleService), baseAddress))
        //    {
        //        var metadataBehavior = new ServiceMetadataBehavior();
        //        metadataBehavior.HttpGetEnabled = true;
        //        metadataBehavior.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
        //        host.Description.Behaviors.Add(metadataBehavior);

        //        host.Open();

        //        Console.WriteLine("The service is ready at {0}", baseAddress);
        //        Console.WriteLine("Press <Enter> to stop the service.");
        //        Console.ReadLine();

        //        host.Close();
        //    }
        //}
    }
}
