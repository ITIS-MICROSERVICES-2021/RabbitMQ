using Microsoft.Extensions.DependencyInjection;
using MSRabbitMQ.Settings;
using RabbitMQ.Client;

namespace MSRabbitMQ.DIExtensions
{
    public static class AddRabbitMQSingletonConnectionExtension
    {
        public static IServiceCollection AddRabbitMQSingletonConnection(this IServiceCollection services,
            RabbitConnectionParameters parameters)
        {
            var connectionFactory = new ConnectionFactory
            {
                UserName = parameters.UserName,
                Password = parameters.Password,
                HostName = parameters.Host,
                Port = parameters.Port
            };

            var connection = connectionFactory.CreateConnection();

            services.AddSingleton(connection);

            return services;
        }
    }
}