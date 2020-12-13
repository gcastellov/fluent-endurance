using System.Threading.Tasks;
using FluentEndurance.Samples.Features;
using Xunit;
using Xunit.Abstractions;

namespace FluentEndurance.Samples
{
    public class EngineTests : BaseTest, IAsyncLifetime
    {
        private readonly EngineFeature _engineFeature;
        private readonly ITestOutputHelper _output;

        public EngineTests(ITestOutputHelper output)
            : base(services => {})
        {
            _output = output;
            _engineFeature = new EngineFeature();
        }

        [Fact]
        public Task EngineShouldStartAndStop()
            => UseFeatureSetGroup(Times.Being(50))
                .WithSet(group => group.Create()
                    .WithStep(_engineFeature, (engine, ct) => engine.Start(ct), Timeout.Being(1200))
                    .WithStep(_engineFeature, (engine, ct) => engine.Stop(ct), Timeout.Being(800)))
                .Run();


        [Fact]
        public Task EngineShouldStartRevAndStop()
            => UseFeatureSetGroup(Times.Being(50))
                .WithSet(group => group.Create()
                    .WithStep(_engineFeature, (engine, ct) => engine.Start(ct), Timeout.Being(1200))
                    .WithStep(_engineFeature, (engine, ct) => engine.Rev3000(ct))
                    .WithStep(_engineFeature, (engine, ct) => engine.Stop(ct), Timeout.Being(800)))
                .Run();

        public Task InitializeAsync()
        {
            // Do nothing
            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            foreach (var notification in _engineFeature.Notifications)
            {
                _output.WriteLine(notification.Content);
            }

            return Task.CompletedTask;
        }
    }
}