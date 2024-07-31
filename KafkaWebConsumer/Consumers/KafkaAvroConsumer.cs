using Confluent.Kafka;
using KafkaLearn.Models;


namespace KafkaWebConsumer.Consumers
{
	internal class KafkaAvroConsumer(
		IConsumer<Null, User> consumer,
		ILogger<KafkaAvroConsumer> logger) : BackgroundService
	{
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			await Task.Yield();

			logger.LogInformation($"{nameof(KafkaAvroConsumer)} start consuming");
			consumer.Subscribe("avro-topic");

			while (!stoppingToken.IsCancellationRequested)
			{
				var consumeResult = consumer.Consume(stoppingToken);
				if (consumeResult is not { IsPartitionEOF: false })
				{
					await Task.Delay(300, stoppingToken);
					continue;
				}

				logger.LogInformation($"Получено сообщение: {consumeResult.Message.Value.name}");

				consumer.Commit();
			}
		}
	}
}
