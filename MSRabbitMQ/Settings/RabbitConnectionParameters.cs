namespace MSRabbitMQ.Settings
{
    public class RabbitConnectionParameters
    {
        public string UserName { get; }
        public string Password { get; }
        public int Port { get; }
        public string Host { get; }
        
        public RabbitConnectionParameters(string userName, string password, int port, string host)
        {
            UserName = userName;
            Password = password;
            Port = port;
            Host = host;
        }
    }
}