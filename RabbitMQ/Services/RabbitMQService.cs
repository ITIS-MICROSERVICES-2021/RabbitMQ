using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ.Services
{
    /// <summary>
    /// Class <c>RabbitMQService</c> is used to publish messages to an exchange and subscribe to receive them
    /// </summary>
    public class RabbitMQService : IRabbitMQService
    {
        private ConnectionFactory ConnectionFactory { get; }

        internal RabbitMQService()
        {
        }

        public RabbitMQService(Uri uri)
        {
            ConnectionFactory = new ConnectionFactory { Uri = uri };
        }

        /// <inheritdoc cref="IRabbitMQService.Publish{T}"/>
        public void Publish<T>(T message, string exchange, string routingKey) where T : new()
        {
            using var connection = ConnectionFactory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange, ExchangeType.Direct);
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            channel.BasicPublish(exchange, routingKey, null, body);
        }

        /// <inheritdoc cref="IRabbitMQService.Subscribe"/>
        public void Subscribe(EventHandler<BasicDeliverEventArgs> eventHandler, string exchange, string routingKey)
        {
            using var connection = ConnectionFactory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange, ExchangeType.Direct);
            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queueName, exchange, routingKey);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += eventHandler;
            channel.BasicConsume(queueName, true, consumer);
        }
    }
}