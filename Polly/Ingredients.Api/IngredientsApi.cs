using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace Ingredients.Api
{
    public class Function1
    {
        private readonly HttpClient _httpClient;

        public Function1()
        {
            _httpClient = new HttpClient();
        }
        [FunctionName("GetIngredients")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            HttpResponseMessage response = new HttpResponseMessage();

            var rand = new Random();
            var next = rand.Next(0, 10);
            log.LogInformation(next);
            if (next % 2 == 1 || next % 4 == 0)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                log.LogCritical("ERROR ERROR");
            }
            else
            {
                response.StatusCode = HttpStatusCode.OK;
                log.LogCritical("All is good");
            }

            return response;
        }
    }
}








// var content = IngredientsApiSpoof.GetIngredients();
// var options = new JsonSerializerOptions()
// {
//     IncludeFields = true,
// };
// var jsonContent = JsonSerializer.Serialize<Ingredient>(content, options);
// var response = new HttpResponseMessage();
// response.Content = new JsonContent(jsonContent);
// response.StatusCode = HttpStatusCode.OK;