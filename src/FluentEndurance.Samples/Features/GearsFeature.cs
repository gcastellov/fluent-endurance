using System.Threading;
using System.Threading.Tasks;

namespace FluentEndurance.Samples.Features
{
    internal class GearsFeature : Feature
    {
        public Task ChangeToNeutral(CancellationToken _) => Task.Delay(500);

        public Task ChangeToDrive(CancellationToken _) => Task.Delay(500);

        public Task ChangeToPark(CancellationToken _) => Task.Delay(500);
    }
}