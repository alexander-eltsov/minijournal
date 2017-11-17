using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using Autofac;
using Nelibur.ServiceModel.Services.Default;
using RabbitMQ.Client;

namespace Infotecs.MiniJournal.Service
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var containerBuilder = new MiniJournalContainerBuilder();
            var container = containerBuilder.Build();

            var connectionFactory = container.Resolve<IConnectionFactory>();
            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var host = new WebServiceHost(typeof(JsonServicePerCall));

                var notificationService = container.Resolve<INotificationService>(new TypedParameter(typeof(IModel), channel));
                notificationService.Initialize();

                try
                {
                    host.Open();

                    Console.WriteLine(host.BaseAddresses.First());
                    Console.WriteLine("Service is running");
                    Console.WriteLine("Press enter to quit...");
                    Console.ReadLine();

                    host.Close();
                }
                catch (CommunicationException cex)
                {
                    Console.WriteLine("An exception occurred: {0}", cex.Message);
                    host.Abort();

                    Console.WriteLine("Press enter to quit...");
                    Console.ReadLine();
                }
            }
        }
    }
}
