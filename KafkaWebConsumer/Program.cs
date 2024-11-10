using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using KafkaLearn.Models;
using KafkaWebConsumer.Consumers;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

//todo попробовать переписать с использованием AddOption. 

builder.Services.Configure<SchemaRegistryConfig>(builder.Configuration.GetSection("KafkaSettings:SchemaRegistry").Bind);
builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection("KafkaSettings:ConsumerSettings").Bind);

var consumerConfig = new ConsumerConfig();
builder.Configuration.GetSection("KafkaSettings:UserConsumerAvro").Bind(consumerConfig);

builder.Services.AddSingleton(sp => new CachedSchemaRegistryClient(sp.GetRequiredService<IOptions<SchemaRegistryConfig>>().Value));
builder.Services.AddSingleton(sp => new ConsumerBuilder<Null, string>(sp.GetRequiredService<IOptions<ConsumerConfig>>().Value).Build());
builder.Services.AddSingleton(sp => new ConsumerBuilder<Null, User>(consumerConfig)
	.SetValueDeserializer(new AvroDeserializer<User>(sp.GetRequiredService<CachedSchemaRegistryClient>()).AsSyncOverAsync())
	.Build());

builder.Services.AddHostedService<KafkaBackgroundConsumer>();
builder.Services.AddHostedService<KafkaAvroConsumer>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();


app.Run();
