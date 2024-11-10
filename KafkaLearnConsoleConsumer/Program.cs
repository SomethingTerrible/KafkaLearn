// Kafka Consumer.


using Confluent.Kafka;
using KafkaLearnConsoleConsumer;
using System.Text.Json;

var consumerConfig = new ConsumerConfig()
{
	BootstrapServers = "localhost:9092",
	EnableAutoCommit = false,
	AllowAutoCreateTopics = false,
	GroupId = "lrn-kafka-group1",
	AutoOffsetReset = AutoOffsetReset.Earliest,
};

using var consumer = new ConsumerBuilder<Null, byte[]>(consumerConfig).Build();

consumer.Subscribe("kafka-test-topic");

var cts = new CancellationTokenSource(); 

while (!cts.IsCancellationRequested)
{
	try
	{
		var consumerResult = consumer.Consume(cts.Token);
		if (consumerResult == null)
		{
			await Task.Delay(100);
			continue;
		}


		var user = JsonSerializer.Deserialize<UserDto>(consumerResult.Message.Value);

        Console.WriteLine($"Получено сообщение: {user!.UserName} {user.Password} {user.Email}");

        consumer.Commit(consumerResult);
	}
	catch (OperationCanceledException ex)
	{
		consumer.Close();
	}
}
consumer.Unsubscribe();
consumer.Close();
consumer.Dispose();

