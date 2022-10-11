using System;
using RabbitMQ.Client;
using System.Text;

class Publisher
{
    private const string Exchange_Topolino = "topolino";

    public static void Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            // Dichiaro l'exchange dove pubblico i miei messaggi e con impostazione Fanout.
            channel.ExchangeDeclare(exchange: Exchange_Topolino, type: ExchangeType.Fanout);

            while (true)
            {

                var message = Console.ReadLine();
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: Exchange_Topolino,
                                     routingKey: "",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }
        }

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }
}