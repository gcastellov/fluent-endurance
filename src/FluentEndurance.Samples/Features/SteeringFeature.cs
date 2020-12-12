using System.Threading;
using System.Threading.Tasks;

namespace FluentEndurance.Samples.Features
{
    internal class SteeringFeature : Feature
    {
        public Task Left(CancellationToken _) => Task.Delay(100);

        public Task Right(CancellationToken _) => Task.Delay(100);

        public Task Forward(CancellationToken _) => Task.Delay(100);
    }
}