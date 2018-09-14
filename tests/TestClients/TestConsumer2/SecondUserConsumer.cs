using System;
using System.Threading.Tasks;
using MassTransit;
using MessageLibrary.Contracts;

namespace TestConsumer2
{
	public class SecondUserConsumer : IConsumer<IAddUser>
	{
		public async Task Consume(ConsumeContext<IAddUser> context)
		{
			await Console.Out.WriteLineAsync("New User Message \n " +
			                                 $"Name: {context.Message.FirstName} {context.Message.LastName}\n " +
			                                 $"Email: {context.Message.EmailAddress}");
		}
	}
}