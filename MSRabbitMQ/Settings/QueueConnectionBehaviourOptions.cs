namespace MSRabbitMQ.Settings
{
    public class QueueConnectionBehaviourOptions
    {
        /// <summary>
        /// Если произошёл перезапуск Rabbit-сервера, то очередь должна оставаться живой 
        /// Default: true
        /// </summary>
        public bool Durable { get; set; } = true;

        /// <summary>
        /// Если единственное соединение с сервером завершится, то нужно удалить очередь
        /// Default: false
        /// </summary>
        public bool Exclusive { get; set; } = false;

        /// <summary>
        /// Если последний потребитель отменит подписку на уведолмения. то нужно удалить очередь
        /// Default: false
        /// </summary>
        public bool AutoDelete { get; set; }
    }
}