using System;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Services;

namespace RabbitMQ.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a RabbitMQService to a dependency injection
        /// </summary>
        /// <param name="services">Services</param>
        /// <param name="user">Authentication parameter username</param>
        /// <param name="pass">Authentication parameter password</param>
        /// <param name="hostName">Host address</param>
        /// <param name="port">5672 for regular ("plain TCP") connections,
        /// 5671 for connections with TLS enabled</param>
        /// <param name="vhost">The namespace for exchanges and queues</param>
        public static void AddRabbitMQ(this IServiceCollection services, string user, string pass, string hostName,
            int port, string vhost)
        {
            var uri = new Uri($"amqp://{user}:{pass}@{hostName}:{port}/{vhost}");
            services.AddSingleton<IRabbitMQService>(new RabbitMQService(uri));
        }
    }
}