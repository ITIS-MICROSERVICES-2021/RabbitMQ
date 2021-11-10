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
        /// <param name="user">Authentication parameter username. Default is "guest"</param>
        /// <param name="pass">Authentication parameter password. Default is "guest"</param>
        /// <param name="hostName">Host address. Default is "localhost"</param>
        /// <param name="port">5672 for regular connections, 5671 for connections with TLS enabled. Default is 5672</param>
        /// <param name="vhost">The namespace for exchanges and queues. Default is "/"</param>
        public static void AddRabbitMQ(this IServiceCollection services, string user = "guest", string pass = "guest",
            string hostName = "localhost", int port = 5672, string vhost = "/")
        {
            var uri = new Uri($"amqp://{user}:{pass}@{hostName}:{port}/{vhost}");
            services.AddSingleton<IRabbitMQService>(new RabbitMQService(uri));
        }
    }
}