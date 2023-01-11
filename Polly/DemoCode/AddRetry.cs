using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Fallback;
using Polly.Retry;
using System.Net;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddRetry(this IServiceCollection services)
    {
        //AddHttpClient with no arguments returns IServiceCollection
        //while AddPolicyHandler applies to IHttpClientBuilder.
        //You will need to use a named client
        services.AddHttpClient();

        services.AddHttpClient<WaffleService>()
                .AddPolicyHandler(GetBasicRetryPolicy());



        return services;
    }

    // Premise "Maybe it's just a blip"

    static IAsyncPolicy<HttpResponseMessage> GetBasicRetryPolicy()
    {
        return HttpPolicyExtensions
            // Handles HttpRequestException,
            // Http status codes >= 500 (server errors) and status code 408 (request timeout)
            .HandleTransientHttpError()
            .RetryAsync(3);
    }

    static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy =
      Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
          .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
          .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));


}
