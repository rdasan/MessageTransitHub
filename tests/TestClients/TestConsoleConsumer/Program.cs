using System;
using MassTransit;

namespace TestConsoleConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
	        Console.WriteLine("Consumer 1\n**************************\n\n");
			var busControl = Bus.Factory.CreateUsingRabbitMq(sbc =>
	        {
		        var rabbitMqHost = sbc.Host(new Uri("rabbitmq://localhost"), host =>
		        {
			        host.Username("guest");
			        host.Password("guest");
		        });
		        sbc.ReceiveEndpoint(rabbitMqHost, "User", e =>
		        {
			        e.Consumer<AddUserConsumer>();
		        });
	        });
	        busControl.Start();
		}
    }
}
