using System.Net;
using System.Text.Json.Serialization;

namespace EurekaDemo.Models
{
	public class DadJokeResponse
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }
		[JsonPropertyName("joke")]
		public string Joke { get; set; }
		[JsonPropertyName("status")]
		public HttpStatusCode Status { get; set; }
	}
}
