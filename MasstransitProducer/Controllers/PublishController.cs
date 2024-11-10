using KafkaLearn.Models;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MasstransitProducer.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class PublishController : ControllerBase
	{
		private readonly ILogger<PublishController> _logger;

		private readonly IPublishEndpoint _publishEndpoint;

		public PublishController(IPublishEndpoint publishEndpoint, ILogger<PublishController> logger)
		{
			_publishEndpoint = publishEndpoint;
			_logger = logger;
		}

		[HttpPost("CreateUser")]
		public async Task<IActionResult> CreateUser([FromBody] User message)
		{
			await _publishEndpoint.Publish<UserCreated>(new
			{
				Id = 1,
				message.LastName,
				message.FirstName,
			});

			return Ok();
		}
	}

	public class User
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }
	}
}
