using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing;

namespace Chat.Core
{
    public class Client
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _roomKey;
        private readonly string _exchange;

        public delegate void MessageHandler(string message);
        public event MessageHandler MessageReceived;

        public Client(string host, string roomKey)
        {
            _roomKey = roomKey;
            _exchange = "chat-exchange";

            ConnectionFactory factory = new ConnectionFactory()
            {
                Uri = new Uri(host)
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue:      _roomKey,
                durable:    true,
                exclusive:  false,
                autoDelete: true,
                arguments:  null);

            _channel.QueueBind(_roomKey, _exchange, _roomKey);

            BeginRead();
        }

        public void SendMessage(string message)
        {
            byte[] body = Encoding.UTF8.GetBytes(message);

            IBasicProperties properties = new BasicProperties
            {
                DeliveryMode = 2,
                Persistent = true
            };

            uint consumerCount = _channel.ConsumerCount(_roomKey);
            for (int i = 0; i < consumerCount; i++)
            {
                _channel.BasicPublish(
                    exchange:        _exchange,
                    routingKey:      _roomKey,
                    basicProperties: properties,
                    body:            body);
            }
        }

        private void BeginRead()
        {
            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, args) =>
            {
                byte[] body = args.Body;
                string message = Encoding.UTF8.GetString(body);

                MessageReceived?.Invoke(message);
            };

            _channel.BasicConsume(
                queue:    _roomKey,
                autoAck:  true,
                consumer: consumer);
        }
    }
}
