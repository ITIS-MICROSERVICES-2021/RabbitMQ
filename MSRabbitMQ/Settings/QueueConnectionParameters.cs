using RabbitMQ.Client;

namespace MSRabbitMQ.Settings
{
    public class QueueConnectionParameters
    {
        public string ExchangeName { get; }
        
        public string ExchangeType { get; }
        public string QueueName { get; }
        public string RoutingKey { get; }
        public QueueConnectionBehaviourOptions BehaviourOptions { get;  }

        public QueueConnectionParameters(string exchangeName, string queueName, string routingKey, string exchangeType, QueueConnectionBehaviourOptions behaviourOptions = null)
        {
            ExchangeName = exchangeName;
            QueueName = queueName;
            RoutingKey = routingKey;
            ExchangeType = exchangeType;
            BehaviourOptions = behaviourOptions ?? new QueueConnectionBehaviourOptions();
        }
    }
}