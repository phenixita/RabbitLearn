using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

class Subscriber
{
    private const string Exchange_Topolino = "topolino";

    public static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };

        Console.WriteLine("Routing key:");
        var routingKey = Console.ReadLine();

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            // Dichiaro quale exchange ascolto.
            channel.ExchangeDeclare(exchange: Exchange_Topolino, type: ExchangeType.Direct);

            // Dichiara un nome random per la coda dove riceverò i miei messaggi in abbonamento.
            var queueName = channel.QueueDeclare(queue: "").QueueName;
            channel.QueueBind(queue: queueName, exchange: Exchange_Topolino, routingKey: routingKey);

            Console.WriteLine(" [*] Waiting for topolino.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] {0}", message);
            };
            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}