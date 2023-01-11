using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Fallback;
using Polly.Retry;
using System.Net;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddFallback(this IServiceCollection services)
    {
        //AddHttpClient with no arguments returns IServiceCollection
        //while AddPolicyHandler applies to IHttpClientBuilder.
        //You will need to use a named client
        services.AddHttpClient();

        services.AddHttpClient<WaffleService>()
                .AddPolicyHandler(FallbackPolicy);


        return services;
    }



    // Premise: 'If all else fails, degrade gracefully'
    static IAsyncPolicy<HttpResponseMessage> FallbackPolicy =
        Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .FallbackAsync(FallbackAction, OnFallbackAsync);

    static Task OnFallbackAsync(DelegateResult<HttpResponseMessage> response, Context context)
    {
        Console.WriteLine(">>>>>>>>>>>>> About to call the fallback action. This is a good place to do some logging");
        return Task.CompletedTask;
    }

    static Task<HttpResponseMessage> FallbackAction(DelegateResult<HttpResponseMessage> responseToFailedRequest,
        Context context, CancellationToken cancellationToken)
    {
        Console.WriteLine(">>>>>>>>>>>>> Fallback action is executing");

        HttpResponseMessage httpResponseMessage = new HttpResponseMessage(responseToFailedRequest.Result.StatusCode)
        {
            Content = new StringContent(
                $"The fallback executed, the original error was {responseToFailedRequest.Result.ReasonPhrase}")
        };
        return Task.FromResult(httpResponseMessage);
    }
}
