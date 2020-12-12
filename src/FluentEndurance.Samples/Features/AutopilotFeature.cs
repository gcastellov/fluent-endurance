using System.Threading;
using System.Threading.Tasks;

namespace FluentEndurance.Samples.Features
{
    internal class AutopilotFeature : Feature
    {
        public Task UnPark(CancellationToken _) => Task.Delay(5000);

        public Task Park(CancellationToken _) => Task.Delay(5000);

        public Task Drive(CancellationToken _) => Task.Delay(10000);
    }
}