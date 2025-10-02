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
    Console.WriteLine(@" Введите точки ... и [enter] чтобы отправить сообщение (""e"" для выхода)");
    var line = Console.ReadLine();
    if (line == "e")
        break;
    
    var message = line?.Length > 0 ? string.Join(" ", line) : "Hello World!";
    var body = Encoding.UTF8.GetBytes(message);

    await channel.BasicPublishAsync(exchange: "", routingKey: "hello", body: body);
    Console.WriteLine($" [x] Sent {message}");
}

Console.WriteLine(" Press [enter] to exit. ");
Console.ReadLine();

static string GetMessage(string[] args)
{
    return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
}