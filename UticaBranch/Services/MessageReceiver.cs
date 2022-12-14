using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace UticaBranch.Services
{
    public class MessageReceiver
    {

        public static List<String> interestedProducts = new List<string>();
        private IServiceScopeFactory scopeFactory;
        private ConnectionFactory connectionFactory = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "Gaga",
            Password = "framework123-.",
            VirtualHost = "NetVHost",
            DispatchConsumersAsync = true
        };
        private IConnection connection;
        private IModel channel;

        public MessageReceiver(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
            connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();
        }

        public void receiveMessages()
        {
            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.Received += async (channel, body) =>
            {

                var messageBytes = body.Body.ToArray();
                string jsonText = Encoding.UTF8.GetString(messageBytes);
                Message result = JsonSerializer.Deserialize<Message>(jsonText);

                await processMessage(result);


            };
            string consumerTag = channel.BasicConsume(queue: "UticaQueue",
                                  autoAck: true,
                                  consumer: consumer);
        }

        private async Task<int> processMessage(Message message)
        {

            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<DatabaseContext>();
                if (interestedProducts.Contains(message.productName))
                {
                    dbContext.Add(message);
                    return await dbContext.SaveChangesAsync();
                }
                return 0;
            }
        }

        public static void addInterestedProduct(String name)
        {
            interestedProducts.Add(name);
        }


    }

    public class Message
    {
        [Key]
        [JsonIgnore]
        public long id { get; set; }

        [JsonRequired]
        public string productName { get; set; }
        public decimal? oldPrice { get; set; }
        public decimal? currentPrice { get; set; }

        [JsonRequired]
        public string? info { get; set; }

        public Message() { }

        public Message(string productName, decimal oldPrice, decimal currentPrice)
        {
            this.productName = productName;
            this.oldPrice = oldPrice;
            this.currentPrice = currentPrice;
        }

        public Message(string productName, string info)
        {
            this.productName = productName;
            this.info = info;
        }
    }

}