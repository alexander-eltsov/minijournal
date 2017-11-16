using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows;
using Infotecs.MiniJournal.Application.Interfaces;
using Infotecs.MiniJournal.Contracts.Notification;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infotecs.MiniJournal.Application
{
    public class RabbitMqNotificationService : INotificationService, IDisposable
    {
        private readonly IModel channel;
        private readonly ILogger logger;
        private const string exchnageName = "MiniJournal";
        private const string exchnageType = "fanout";
        private readonly Subject<NotificationMessage> subject;
        private readonly Dictionary<object, Subject<bool>> stopsDictionary;
        private EventingBasicConsumer consumer;

        public RabbitMqNotificationService(IModel channel, ILogger logger)
        {
            this.channel = channel;
            this.logger = logger;
            subject = new Subject<NotificationMessage>();
            stopsDictionary = new Dictionary<object, Subject<bool>>();
        }

        public void Initialize()
        {
            if (this.consumer != null)
            {
                return;
            }

            channel.ExchangeDeclare(
                exchange: exchnageName,
                type: exchnageType);

            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(
                queue: queueName,
                exchange: exchnageName,
                routingKey: "");

            consumer = new EventingBasicConsumer(channel);

            SubscribeConsumerEvent();

            channel.BasicConsume(
                queue: queueName,
                autoAck: true,
                consumer: consumer);
        }

        public void NotifyError(string error)
        {
            logger.LogError(error);
            MessageBox.Show(error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void NotifyError(Exception exception)
        {
            logger.LogError(exception);
            MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public IObservable<TEvent> GetEvent<TEvent>() where TEvent : NotificationMessage
        {
            return subject.OfType<TEvent>().AsObservable();
        }

        public void Subscribe<TMessage>(Action<TMessage> handler) where TMessage : NotificationMessage
        {
            Unsubscribe(handler);

            var stop = new Subject<bool>();
            stopsDictionary.Add(handler, stop);

            subject.OfType<TMessage>()
                .AsObservable()
                .TakeUntil(stop)
                .Subscribe(handler);
        }

        public void Unsubscribe<TMessage>(Action<TMessage> handler) where TMessage : NotificationMessage
        {
            if (!stopsDictionary.ContainsKey(handler))
            {
                return;
            }

            try
            {
                Subject<bool> stop = stopsDictionary[handler];
                stop.OnNext(true);
                stop.Dispose();
                stopsDictionary.Remove(handler);
            }
            catch (ObjectDisposedException)
            {
            }
        }

        public void Dispose()
        {
            subject?.Dispose();
            UnsubscribeConsumerEvent();
        }

        private void OnConsumerReactive(object sender, BasicDeliverEventArgs args)
        {
            byte[] body = args.Body;
            var serializer = new NotificationMessageSerializer();
            NotificationMessage notificationMessage = serializer.Deserialize(body);
            subject.OnNext(notificationMessage);
        }

        private void SubscribeConsumerEvent()
        {
            consumer.Received += OnConsumerReactive;
        }

        private void UnsubscribeConsumerEvent()
        {
            consumer.Received -= OnConsumerReactive;
        }
    }
}