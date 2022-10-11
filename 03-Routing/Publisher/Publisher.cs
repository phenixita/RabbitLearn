using System;
using RabbitMQ.Client;
using System.Text;

class Publisher
{
    private const string Exchange_Topolino = "topolino";

    public static void Main(string[] args)
    {
        Console.WriteLine("Scrivi numeri:");
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            // Dichiaro l'exchange dove pubblico i miei messaggi e con impostazione Fanout.
            channel.ExchangeDeclare(exchange: Exchange_Topolino, type: ExchangeType.Direct);

            while (true)
            {
                var message = Console.ReadLine();
                var pariDispari = int.Parse(message) % 2 == 0 ? "pari" : "dispari";
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: Exchange_Topolino,
                                     routingKey: pariDispari,
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }
        }

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }
}