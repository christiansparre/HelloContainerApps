using System;
using System.Threading.Tasks;
using HelloContainerApps.Shared;
using Orleans;

namespace HelloContainerApps.Silo.Grains
{
    public class HelloWorldGrain : Grain, IHelloWorldGrain
    {
        public Task<string> Hello(string name)
        {
            return Task.FromResult($"Hello {name}. The time and date on my end is \"{DateTime.Now:F}\" and my hostname is \"{Environment.MachineName}\"");
        }
    }
}
