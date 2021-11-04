using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace HelloContainerApps.Client.Services
{
    public class ClusterClientHostedService : IHostedService
    {
        private readonly ILogger<ClusterClientHostedService> _logger;
        private readonly IConfiguration _configuration;

        public ClusterClientHostedService(ILogger<ClusterClientHostedService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            Client = new ClientBuilder()
                .UseAzureStorageClustering(options =>
                {
                    options.ConnectionString = _configuration["AzureStorageConnectionString"];
                    options.TableName = _configuration["AzureStorageTableName"];
                })
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "HelloWorld";
                    options.ServiceId = "HelloWorld";
                })
                .Build();
        }

        public IClusterClient Client { get; set; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Connecting...");
            var retries = 100;
            await Client.Connect(async error =>
            {
                if (--retries < 0)
                {
                    _logger.LogError("Could not connect to the cluster: {@Message}", error.Message);
                    return false;
                }
                else
                {
                    _logger.LogWarning(error, "Error Connecting: {@Message}", error.Message);
                }

                try
                {
                    await Task.Delay(1000, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    return false;
                }

                return true;
            });

            _logger.LogInformation("Connected {Initialized}", Client.IsInitialized);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            var cancellation = new TaskCompletionSource<bool>();
            cancellationToken.Register(() => cancellation.TrySetCanceled(cancellationToken));

            return Task.WhenAny(Client.Close(), cancellation.Task);
        }

    }
}
