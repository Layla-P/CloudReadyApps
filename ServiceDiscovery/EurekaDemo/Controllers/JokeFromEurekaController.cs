
using Microsoft.AspNetCore.Mvc;

namespace EurekaDemo.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class JokeFromEurekaController : ControllerBase
	{
		private readonly HttpClient _client;
		public JokeFromEurekaController(IHttpClientFactory clientFactory)
		{
			_client = clientFactory.CreateClient("DiscoveryRoundRobin");
		}

		[HttpGet]
		public async Task<string> Get()
		{			
			return await _client.GetStringAsync(new Uri("https://dadjokeapi/DadJoke"));
		}
	}
}
