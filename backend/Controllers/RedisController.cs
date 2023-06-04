// create controller class with post and get endpoints for setting and getting value using RedisService singleton defined in program.cs and injected into the controller

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace project.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class RedisController : ControllerBase
	{
		private readonly ILogger<RedisController> _logger;
		private readonly RedisService _redisService;

		public RedisController(ILogger<RedisController> logger, RedisService redisService)
		{
			_logger = logger;
			_redisService = redisService;
		}

		[HttpPost(Name = "SetRedisValue")]
		public void Post([FromBody] RedisRequest request)
		{
			_redisService.SetValue(request.Key, request.Value);
		}

		[HttpGet(Name = "GetRedisValue")]
		public string Get([FromQuery] string key)
		{
			var result = new
			{
				key = key,
				value = _redisService.GetValue(key)
			};
			return JsonConvert.SerializeObject(result);
		}
	}
}
