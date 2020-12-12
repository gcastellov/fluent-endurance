using System.Threading;
using System.Threading.Tasks;

namespace FluentEndurance.Samples.Features
{
    internal class EngineFeature : Feature
    {
        public Task Start(CancellationToken _) => Task.Delay(1000);

        public Task Rev3000(CancellationToken _) => Task.Delay(500);

        public Task Accelerate100(CancellationToken _) => Task.Delay(1000);

        public Task Accelerate150(CancellationToken _) => Task.Delay(500);

        public Task Stop(CancellationToken _) => Task.Delay(50);
    }
}