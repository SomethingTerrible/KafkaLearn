using KafkaLearn.Models;
using MassTransit;

namespace MasstransitConsumer
{
	public class UserCreatedConsumer: IConsumer<UserCreated>
	{
		private readonly ILogger<UserCreatedConsumer> _logger;

		public UserCreatedConsumer(ILogger<UserCreatedConsumer> logger)
		{
			_logger = logger;
		}

		public Task Consume(ConsumeContext<UserCreated> context)
		{
			_logger.LogInformation($"{context.Message.Id} {context.Message.FirstName} {context.Message.LastName}");

			context.Publish<UserSaved>(new UserSaved { FirstName = context.Message.FirstName });

			return Task.CompletedTask;
        }
	}

}
