using Polly;
using Polly.CircuitBreaker;
using System.Net;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddCircuitBreaker(this IServiceCollection services)
    {
        //AddHttpClient with no arguments returns IServiceCollection
        //while AddPolicyHandler applies to IHttpClientBuilder.
        //You will need to use a named client
        services.AddHttpClient();

        services.AddHttpClient<WaffleService>()
                .AddPolicyHandler(CircuitBreakerPolicy);



        return services;
    }

    // "Stop doing it if it hurts"

    // "Give that system a break"

    public static readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> CircuitBreakerPolicy =
    Policy
        .HandleResult<HttpResponseMessage>
            (message => message.StatusCode == HttpStatusCode.InternalServerError)
        .CircuitBreakerAsync(3, TimeSpan.FromSeconds(20), OnBreak, OnReset, OnHalfOpen);


    static void OnHalfOpen()
    {
        Console.WriteLine(">>>>>>>>>>>>> Circuit in test mode, one request will be allowed.");
    }

    static void OnReset()
    {
        Console.WriteLine(">>>>>>>>>>>>> Circuit closed, requests flow normally.");
    }

    static void OnBreak(DelegateResult<HttpResponseMessage> result, TimeSpan ts)
    {
        Console.WriteLine(">>>>>>>>>>>>> Circuit cut, requests will not flow.");
    }

}
