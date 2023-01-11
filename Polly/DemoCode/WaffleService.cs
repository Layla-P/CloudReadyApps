

using System.Text.Json;

public class WaffleService
{
    private readonly HttpClient _httpClient;

    public WaffleService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> Get()
    {

        var response = await _httpClient.GetAsync("http://localhost:7198/api/GetIngredients");

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine(">>>>>>>>>>>>> response is A-okay!");
            return "Success";
        }
        //handle error here
        return "Failure";

    }
}

