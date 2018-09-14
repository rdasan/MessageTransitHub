using System;
using MassTransit;

namespace TestConsumer2
{
    class Program
    {
        static void Main(string[] args)
        {
			Console.WriteLine("Consumer 2\n**************************\n\n");
			var busControl = Bus.Factory.CreateUsingRabbitMq(sbc =>
	        {
		        var rabbitMqHost = sbc.Host(new Uri("rabbitmq://localhost"), host =>
		        {
			        host.Username("guest");
			        host.Password("guest");
		        });
		        sbc.ReceiveEndpoint(rabbitMqHost, "User", e =>
		        {
			        e.Consumer<SecondUserConsumer>();
		        });
	        });
	        busControl.Start();
		}
    }
}
