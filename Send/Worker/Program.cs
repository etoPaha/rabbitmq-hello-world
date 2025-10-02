﻿using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

// создаем очередь
await channel.QueueDeclareAsync(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

Console.WriteLine(" [*] Waiting for messages... ");

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += async (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    
    Console.WriteLine($" [x] Received {message}");

    int dots = message.Split('.').Length - 1;
    await Task.Delay(dots * 10000);

    Console.WriteLine(" [x] Done");
    
    // ручное подтверждение
    await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
};

await channel.BasicConsumeAsync("hello", autoAck: false, consumer: consumer);

Console.WriteLine(" Press [enter] to exit. ");
Console.ReadLine();