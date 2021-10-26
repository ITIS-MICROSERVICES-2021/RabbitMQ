using Microsoft.Extensions.DependencyInjection;
using MSRabbitMQ.Settings;
using RabbitMQ.Client;

namespace MSRabbitMQ.DIExtensions
{
    public static class AddRabbitMQTransientChannelExtension
    {
        public static IServiceCollection AddRabbitMQTransientChannel(this IServiceCollection services, 
            RabbitConnectionParameters rabbitConnectionParameters, 
            QueueConnectionParameters queueConnectionParameters )
        {
            services.AddTransient<IModel>(_ =>
            {
                var connectionFactory = new ConnectionFactory
                {
                    UserName = rabbitConnectionParameters.UserName,
                    Password = rabbitConnectionParameters.Password,
                    HostName = rabbitConnectionParameters.Host,
                    Port = rabbitConnectionParameters.Port
                };

                var connection = connectionFactory.CreateConnection();

                var model = connection.CreateModel();
                model.ExchangeDeclare(queueConnectionParameters.ExchangeName, queueConnectionParameters.ExchangeType);
                model.QueueDeclare(queueConnectionParameters.QueueName,
                    queueConnectionParameters.BehaviourOptions.Durable,
                    queueConnectionParameters.BehaviourOptions.Exclusive,
                    queueConnectionParameters.BehaviourOptions.AutoDelete);
                
                model.QueueBind(queueConnectionParameters.QueueName, queueConnectionParameters.ExchangeName, queueConnectionParameters.RoutingKey, null);
                return model;
            });
            
            return services;
        }
    }
}