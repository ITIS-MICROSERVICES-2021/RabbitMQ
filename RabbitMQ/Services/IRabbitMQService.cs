using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;

namespace RabbitMQ.Services
{
    /// <summary>
    /// Interface <c>IRabbitMQService</c> is used to publish messages to an exchange and subscribe to receive them
    /// </summary>
    public interface IRabbitMQService
    {
        /// <summary>
        /// Method <c>Publish</c> publishes a message to an exchange
        /// </summary>
        /// <param name="message">Message that is sent</param>
        /// <param name="exchange">Entity where message is sent to</param>
        /// <param name="routingKey">Route to zero or more queues</param>
        /// <typeparam name="T">Message type</typeparam>
        public void Publish<T>(T message, string exchange, string routingKey) where T : new();

        /// <summary>
        /// Method <c>Subscribe</c> sets up a subscription to receive messages
        /// </summary>
        /// <param name="action">Handler of received message</param>
        /// <param name="exchange">Entity where message is sent to</param>
        /// <param name="routingKey">Route to zero or more queues</param>
        /// <param name="logger">Logger</param>
        /// <typeparam name="T">Message type</typeparam>
        public void Subscribe<T>(Action<T> action, string exchange, string routingKey, ILogger logger);

        /// <summary>
        /// Method <c>Subscribe</c> sets up a subscription to receive messages
        /// </summary>
        /// <param name="func">AsyncHandler of received message</param>
        /// <param name="exchange">Entity where message is sent to</param>
        /// <param name="routingKey">Route to zero or more queues</param>
        /// <param name="logger">Logger</param>
        /// <typeparam name="T">Message type</typeparam>
        public void Subscribe<T>(Func<T, Task> func, string exchange, string routingKey, ILogger logger);
    }
}