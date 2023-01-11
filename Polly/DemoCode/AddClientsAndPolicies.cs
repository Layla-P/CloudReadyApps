using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Fallback;
using Polly.Retry;
using System.Net;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddClientsAndPolicies(this IServiceCollection services)
    {
        IAsyncPolicy<HttpResponseMessage> wrapOfRetryAndFallback =
           Policy.WrapAsync(FallbackPolicy, GetRetryPolicy, CircuitBreakerPolicy);


        services.AddHttpClient();

        services.AddHttpClient<WaffleService>()
                .AddPolicyHandler(wrapOfRetryAndFallback);



        return services;
    }


}
