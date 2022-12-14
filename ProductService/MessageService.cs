using System;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace ProductService
{
    public class MessageSender
    {
        private static ConnectionFactory connectionFactory = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "Gaga",
            Password = "framework123-.",
            VirtualHost = "NetVHost"
        };
        private static IConnection connection = connectionFactory.CreateConnection();
        private static IModel channel = connection.CreateModel();
        public MessageSender()
        {
            channel.ExchangeDeclare("ProductExchange", ExchangeType.Fanout, true, false);
        }

        public static void sendMessage(Object message)
        {
            var body = new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message)));
            string jsonText = Encoding.UTF8.GetString(body.ToArray());
            channel.BasicPublish(exchange: "ProductExchange",
                                 routingKey: "ProductEvents",
                                 body: body);



        }




    }
}
