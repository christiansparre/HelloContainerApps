using System;
using System.Threading.Tasks;
using HelloContainerApps.Shared;
using Microsoft.Extensions.Logging;
using Orleans;

namespace HelloContainerApps.Silo.Grains
{
    [CollectionAgeLimit(Minutes = 5)]
    public class HelloWorldGrain : Grain, IHelloWorldGrain
    {
        private readonly ILogger<HelloWorldGrain> _logger;

        public HelloWorldGrain(ILogger<HelloWorldGrain> logger)
        {
            _logger = logger;
        }

        public Task<string> Hello(string name)
        {
            _logger.LogInformation("Received hello call from \"{Name}\"", name);
            return Task.FromResult($"Hello {name}. The date and time on my end is \"{DateTime.Now:F}\" and my hostname is \"{Environment.MachineName}\"");
        }
    }
}
    