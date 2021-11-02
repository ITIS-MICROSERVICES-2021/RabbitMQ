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
        /// <param name="services"></param>
        /// <param name="uri">Uri that is used to connect to a RabbitMQ node in "amqp://user:pass@hostName:port/vhost format"</param>
        public static void AddRabbitMQ(this IServiceCollection services, Uri uri)
        {
            services.AddSingleton<IRabbitMQService>(new RabbitMQService(uri));
        }
    }
}