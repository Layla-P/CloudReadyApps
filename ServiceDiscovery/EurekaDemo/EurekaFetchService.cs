using Spectre.Console;
using Steeltoe.Discovery;
using Steeltoe.Discovery.Eureka;

namespace EurekaDemo
{
    internal class EurekaFetchService : IHostedService
    {
        private readonly DiscoveryClient _discoveryClient;
        private readonly ILogger<EurekaFetchService> _logger;
        private Timer _timer;
        private readonly HttpClient _httpClient;
        private bool isFirstTime = true;

        public EurekaFetchService(IDiscoveryClient discoveryClient, HttpClient client, ILogger<EurekaFetchService> logger)
        {
            _discoveryClient = discoveryClient as DiscoveryClient;
            _httpClient = client;
            _logger = logger;
        }


        public async Task UseDiscoveryClient()
        {

            var apps = _discoveryClient.Applications.GetRegisteredApplications();

            var count = 0;

            _logger.LogInformation(@$"There are {count} apps registered with Eureka");

            
            foreach (var app in apps)
            {
                var firstInstance = app.Instances[0];
                var secondInstance = app.Instances[1];

                if (isFirstTime)
                {
                    var rows = new List<Text>(){
                            new Text(@$"First Instance => Name:{app.Name} and hostname: {firstInstance.HostName} and 
		                        port:{firstInstance.SecurePort}", new Style(Color.Red)),
                            new Text($"Second Instance => Name:{app.Name} and hostname: {secondInstance.HostName} and \r\n\t\tport:{secondInstance.SecurePort}", new Style(Color.Green))};


                    AnsiConsole.Write(new Rows(rows));

                }
                isFirstTime = false;
                count++;
                try
                {
                    var random = new Random();
                    var r = random.Next(0, 2);

                    if (app.Name.ToLower() == "dadjokeapi" && !string.IsNullOrEmpty(firstInstance.HomePageUrl))
                    {
                        string url = (r == 1) ? firstInstance.HomePageUrl : secondInstance.HomePageUrl;
                        AnsiConsole.Write(new Rows(new Text(" ")));
                        AnsiConsole.Markup($"[bold lime]{url}dadjoke[/]");
                        var response = await _httpClient.GetStringAsync($"{url}dadjoke");
                        AnsiConsole.Write(new Rows(new Text(" ") ));
                        AnsiConsole.Markup($"[bold yellow]:rolling_on_the_floor_laughing: => {response}[/]");
                        AnsiConsole.Write(new Rows(new Text(" ")));
                        AnsiConsole.Write(new Rows(new Text(" ")));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }

            }



        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _discoveryClient.ShutdownAsync();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Update, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        private async void Update(object state)
        {
            await UseDiscoveryClient();
        }

    }
}