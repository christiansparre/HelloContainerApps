using System.Threading.Tasks;
using Orleans;

namespace HelloContainerApps.Shared
{
    public interface IHelloWorldGrain : IGrainWithGuidKey
    {
        Task<string> Hello(string name);
    }
}