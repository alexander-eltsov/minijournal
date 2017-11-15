using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
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
            //using (var stream = new MemoryStream())
            //{
            //    var serializer = new DataContractJsonSerializer(typeof(NotificationMessage),
            //        new List<Type>
            //        {
            //            typeof(TestMessage),
            //            typeof(ArticleChangedMessage)
            //        });
            //    serializer.WriteObject(stream, message);

            //    Encoding encoding = Encoding.UTF8;
            //    byte[] body = encoding.GetBytes(encoding.GetString(stream.ToArray()));
            //    channel.BasicPublish(
            //        exchange: exchnageName,
            //        routingKey: "",
            //        basicProperties: null,
            //        body: body);
            //}

            var serializer = new NotificationMessageSerializer();
            byte[] body = serializer.Serialize(message);
            channel.BasicPublish(
                    exchange: exchnageName,
                    routingKey: "",
                    basicProperties: null,
                    body: body);

        }

        //public void NotifyArticle()
        //{
        //    using (var stream = new MemoryStream())
        //    {
        //        //var notify = new NotifyArticleChanged
        //        //{
        //        //    ArticleId = 123
        //        //};

        //        var notify = new TestMessage
        //        {
        //            SomeData = "test!"
        //        };
        //        //var wrapperNotify = new NotificationMessage
        //        //{
        //        //    NotificationType = notify.GetType().Name,
        //        //    NotificationItem = notify
        //        //};

        //        //var serializer = new DataContractJsonSerializer(wrapperNotify.GetType(), new List<Type>{ typeof(NotificationChildMessage) });
        //        //serializer.WriteObject(stream, wrapperNotify);

        //        var serializer = new DataContractJsonSerializer(typeof(NotificationMessage), new List<Type> { typeof(TestMessage) });
        //        serializer.WriteObject(stream, notify);


        //        Encoding encoding = Encoding.UTF8;
        //        byte[] body = encoding.GetBytes(encoding.GetString(stream.ToArray()));
        //        channel.BasicPublish(
        //            exchange: exchnageName,
        //            routingKey: "",
        //            basicProperties: null,
        //            body: body);
        //    }
        //}

    }
}