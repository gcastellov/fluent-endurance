using System.Threading;
using System.Threading.Tasks;

namespace FluentEndurance.Tests
{
    public class FeatureTest : Feature
    {
        public virtual Task Do(CancellationToken _) => Task.Delay(1000);
    }
}