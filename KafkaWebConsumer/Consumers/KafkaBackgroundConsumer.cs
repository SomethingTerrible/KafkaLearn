using Confluent.Kafka;

namespace KafkaWebConsumer.Consumers
{
	public class KafkaBackgroundConsumer(IConsumer<Null, string> consumer,
		ILogger<KafkaBackgroundConsumer> logger) : BackgroundService
	{
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			await Task.Yield();

			consumer.Subscribe("kafka-test-topic");

			logger.LogInformation($"{nameof(KafkaBackgroundConsumer)} start consuming");
			try
			{
				while (!stoppingToken.IsCancellationRequested)
				{
					var consumeResult = consumer.Consume(stoppingToken);
					if (consumeResult is not { IsPartitionEOF: false })
					{
						await Task.Delay(300, stoppingToken);
						continue;
					}

					logger.LogInformation($"Получено сообщение: {consumeResult.Message.Value}");

					consumer.Commit();
				}
			}
			catch (OperationCanceledException ex)
			{
				logger.LogError($"Консумер был остановлен: {ex}");

				consumer.Unsubscribe();
				consumer.Close();
				consumer.Dispose();
			}
			catch (ConsumeException ex)
			{
				logger.LogInformation($"{ex.Message}");
			}
		}
	}
}
