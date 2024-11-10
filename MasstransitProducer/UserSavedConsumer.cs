using KafkaLearn.Models;
using MassTransit;

namespace MasstransitProducer
{
	public class UserSavedConsumer : IConsumer<UserSaved>
	{
		private readonly ILogger<UserSavedConsumer> _logger;

		public UserSavedConsumer(ILogger<UserSavedConsumer> logger)
		{
			_logger = logger;
		}

		public Task Consume(ConsumeContext<UserSaved> context)
		{
			_logger.LogInformation(context.Message.FirstName);

			return Task.CompletedTask;
		}
	}
}
