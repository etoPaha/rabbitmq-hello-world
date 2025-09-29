using System.Text;
using RabbitMQ.Client;

// создаем канал
// создаем очередь

var factory = new ConnectionFactory
{
    HostName = "localhost"
};
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

const string message = "Hello World!";
var body = Encoding.UTF8.GetBytes(message);

Console.ReadLine();