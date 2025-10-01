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

// объявляем очередь hello
await channel.QueueDeclareAsync(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

while (true)
{
    Console.WriteLine(@" ""e"" для выхода / [enter] отправить сообщение ");
    var line = Console.ReadLine();
    if (line == "e")
        break;
    
    const string message = "Hello World!";
    var body = Encoding.UTF8.GetBytes(message);

    await channel.BasicPublishAsync(exchange: "", routingKey: "hello", body: body);
    Console.WriteLine($" [x] Sent {message}");
}

Console.WriteLine(" Press [enter] to exit. ");
Console.ReadLine();