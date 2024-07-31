using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using KafkaLearn.Models;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder()
	.AddJsonFile("appsettings.json");

IConfiguration config = builder.Build();

var schemaregistryConfig = new SchemaRegistryConfig();
config.GetSection("KafkaSettings:SchemaRegistry").Bind(schemaregistryConfig);

var producerConfig = new ProducerConfig();
config.GetSection("KafkaSettings:ProducerConfig").Bind(producerConfig);

var avroSerializerConfig = new AvroSerializerConfig
{
	BufferBytes = 100,
	AutoRegisterSchemas = true,
};


using (var schemaRegistry = new CachedSchemaRegistryClient(schemaregistryConfig))
using (var producer = new ProducerBuilder<Null, User>(producerConfig)
	.SetValueSerializer(new AvroSerializer<User>(schemaRegistry, avroSerializerConfig))
	.Build())
{
	while (true)
	{
		var line  = Console.ReadLine();
		if (line != null && line != "exit")
		{
			var user = new User { name = line, favorite_color = "green", favorite_number = 5, hourly_rate = new Avro.AvroDecimal(67.99) };

			await producer.ProduceAsync("avro-topic", new Message<Null, User> { Value = user });
		}
	}
}