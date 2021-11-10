using System;
using System.Text;
using Microsoft.Extensions.Logging;
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

        /// <exception cref="ArgumentNullException">Exception that is thrown when logger is null</exception>
        /// <inheritdoc cref="IRabbitMQService.Subscribe{T}"/>
        public void Subscribe<T>(Action<T> action, string exchange, string routingKey, ILogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger), "Logger can not be null");
            using var connection = ConnectionFactory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange, ExchangeType.Direct);
            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queueName, exchange, routingKey);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (_, args) =>
            {
                var body = args.Body.ToArray();
                try
                {
                    var message = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(body));
                    action(message);
                }
                catch (JsonException e)
                {
                    logger.LogError(e, e.Message);
                    throw;
                }
            };
            channel.BasicConsume(queueName, true, consumer);
        }
    }
}