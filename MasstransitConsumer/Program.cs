using MassTransit;
using MasstransitConsumer;

var builder = WebApplication.CreateBuilder();
builder.Services.AddLogging();

builder.Services.AddMassTransit(x =>
{
	x.SetKebabCaseEndpointNameFormatter();

	x.AddConsumer<UserCreatedConsumer>();

	x.UsingRabbitMq((context, cfg) =>
	{
		cfg.Host("localhost", "/", h =>
		{
			h.Username("guest");
			h.Password("guest");
		});

		cfg.ConfigureEndpoints(context);

		/*cfg.ReceiveEndpoint("user-created", x =>
		{
			x.ConfigureConsumeTopology = false;

			x.Consumer<UserCreatedConsumer>();
		});*/
	});
});


var app = builder.Build();

app.Run();