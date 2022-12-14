using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ScrantonBranch.Services
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
            string consumerTag = channel.BasicConsume(queue: "ScrantonQueue",
                                  true,
                                  consumer: consumer);
        }

        private async Task<int> processMessage(Message message)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<DatabaseContext>();
                if (interestedProducts.Contains(message.name))
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
        public string name { get; set; }

        public string message { get; set; }
        public decimal? oldPrice { get; set; }
        public decimal? currentPrice { get; set; }

        public Message() { }

        public Message(string name, string message, decimal oldPrice, decimal currentPrice)
        {
            this.name = name;
            this.message = message;
            this.oldPrice = oldPrice;
            this.currentPrice = currentPrice;
        }
    }

}