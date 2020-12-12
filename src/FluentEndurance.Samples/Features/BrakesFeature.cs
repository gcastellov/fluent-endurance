using System.Threading;
using System.Threading.Tasks;

namespace FluentEndurance.Samples.Features
{
    internal class BrakesFeature : Feature
    {
        public Task BrakeTo0(CancellationToken _) => Task.Delay(2000);

        public Task BrakeTo50(CancellationToken _) => Task.Delay(2000);
    }
}