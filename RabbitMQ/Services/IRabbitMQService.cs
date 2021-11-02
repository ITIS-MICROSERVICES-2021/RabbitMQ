using System;
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
        /// <param name="message">Message</param>
        /// <param name="exchange">Entity where message is sent to</param>
        /// <param name="routingKey">Route to zero or more queues</param>
        /// <typeparam name="T">Message type</typeparam>
        public void Publish<T>(T message, string exchange, string routingKey) where T : new();

        /// <summary>
        /// Method <c>Subscribe</c> sets up a subscription to receive messages
        /// </summary>
        /// <param name="eventHandler">Handler that is raised when message is received</param>
        /// <param name="exchange">Entity where message is sent to</param>
        /// <param name="routingKey">Route to zero or more queues</param>
        public void Subscribe(EventHandler<BasicDeliverEventArgs> eventHandler, string exchange, string routingKey);
    }
}