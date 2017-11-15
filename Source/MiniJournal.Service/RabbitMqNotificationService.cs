using System;
using Infotecs.MiniJournal.Contracts.Notification;
using RabbitMQ.Client;

namespace Infotecs.MiniJournal.Service
{
    public class RabbitMqNotificationService : INotificationService
    {
        private readonly IModel channel;
        private const string exchnageName = "MiniJournal";
        private const string exchnageType = "fanout";

        public RabbitMqNotificationService(IModel channel)
        {
            this.channel = channel;
        }

        public void Initialize()
        {
            channel.ExchangeDeclare(
                exchange: exchnageName,
                type: exchnageType);
        }

        public void Notify(NotificationMessage message)
        {
            var serializer = new NotificationMessageSerializer();
            byte[] body = serializer.Serialize(message);
            channel.BasicPublish(
                    exchange: exchnageName,
                    routingKey: "",
                    basicProperties: null,
                    body: body);

        }
    }
}