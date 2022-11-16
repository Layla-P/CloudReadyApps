using EurekaDemo.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace EurekaDemo.Services
{
	public class DadJokeService
	{
		private HttpClient _client;
		public DadJokeService(HttpClient client)
		{
			_client = client;
		}
		public async Task<string> GetJokeAsync()
		{
			_client.DefaultRequestHeaders.Add("Accept", "application/json");

			var joke = await _client.GetFromJsonAsync<DadJokeResponse>("https://icanhazdadjoke.com/");

			return joke.Joke;
		}

		
	}
}
