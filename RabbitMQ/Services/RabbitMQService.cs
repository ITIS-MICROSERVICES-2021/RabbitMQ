using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ.Services
{
    /// <summary>
    /// Class <c>RabbitMQService</c> is used to publish messages to an exchange and subscribe to receive them
    /// </summary>
    public class RabbitMQService : IRabbitMQService, IDisposable
    {
        private IConnection Connection { get; }
        private IModel Channel { get; }

        internal RabbitMQService()
        {
        }

        public RabbitMQService(Uri uri)
        {
            var connectionFactory = new ConnectionFactory
            {
                Uri = uri,
                DispatchConsumersAsync = true
            };
            Connection = connectionFactory.CreateConnection();
            Channel = Connection.CreateModel();
        }

        /// <inheritdoc cref="IRabbitMQService.Publish{T}"/>
        public void Publish<T>(T message, string exchange, string routingKey) where T : new()
        {
            Channel.ExchangeDeclare(exchange, ExchangeType.Direct, false, true);
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            Channel.BasicPublish(exchange, routingKey, null, body);
        }

        /// <exception cref="ArgumentNullException">Exception that is thrown when logger is null</exception>
        /// <inheritdoc>
        ///     <cref>IRabbitMQService.Subscribe{T}</cref>
        /// </inheritdoc>
        public void Subscribe<T>(Action<T> action, string exchange, string routingKey, ILogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger), "Logger can not be null");
            Channel.ExchangeDeclare(exchange, ExchangeType.Direct, false, true);
            Channel.QueueDeclare(routingKey);
            Channel.QueueBind(routingKey, exchange, routingKey);
            var consumer = new EventingBasicConsumer(Channel);
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
            Channel.BasicConsume(routingKey, true, consumer);
        }

        /// <exception cref="ArgumentNullException">Exception that is thrown when logger is null</exception>
        /// <inheritdoc>
        ///     <cref>IRabbitMQService.Subscribe{T}</cref>
        /// </inheritdoc>
        public void Subscribe<T>(Func<T, Task> func, string exchange, string routingKey, ILogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger), "Logger can not be null");
            Channel.ExchangeDeclare(exchange, ExchangeType.Direct, false, true);
            Channel.QueueDeclare(routingKey);
            Channel.QueueBind(routingKey, exchange, routingKey);
            var consumer = new AsyncEventingBasicConsumer(Channel);
            consumer.Received += async (_, args) =>
            {
                var body = args.Body.ToArray();
                try
                {
                    var message = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(body));
                    await func(message);
                }
                catch (JsonException e)
                {
                    logger.LogError(e, e.Message);
                    throw;
                }
            };
            Channel.BasicConsume(routingKey, true, consumer);
        }

        public void Dispose()
        {
            Connection.Dispose();
            Channel.Dispose();
        }
    }
}