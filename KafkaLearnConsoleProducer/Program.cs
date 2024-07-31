// Kafka Producer

using Confluent.Kafka;
using KafkaLearnConsoleProduce;
using System.ComponentModel.Design;
using System.Text;
using System.Text.Json;



var producerConfig = new ProducerConfig()
{
	BootstrapServers = "localhost:9092",
	Acks = Acks.All,
	AllowAutoCreateTopics = false,
};
var cts = new CancellationTokenSource();

using var producer = new ProducerBuilder<Null, byte[]>(producerConfig).Build();

Console.WriteLine("Введите сообщение, которое хотите передать \n");

while (true)
{
	var line = Console.ReadLine();
	if (line != null && line != "exit")
	{
		try
		{
			var userDto = new UserDto(line, "password", "email");
			var jsonUser = JsonSerializer.Serialize(userDto);
			var userByteArray = Encoding.UTF8.GetBytes(jsonUser);

			var produceResult = await producer.ProduceAsync("kafka-test-topic", new Message<Null, byte[]> {  Value = userByteArray }, cts.Token);

			Console.WriteLine($"Сообщение было доставлено. \n");
		}
		catch (ProduceException<Null, string> ex)
		{
			Console.WriteLine($"При отправки сообщение произошла ошибка \n {ex.Message}");
			break;
		}
	}
	else
	{
		break;
	}
}

producer.Flush();
producer.Dispose();
