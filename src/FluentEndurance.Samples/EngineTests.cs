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
            : base(_ => {}, services => {})
        {
            _output = output;
            _engineFeature = new EngineFeature();
        }

        [Fact]
        public Task EngineShouldStartAndStop()
            => UseFeatureSetGroup().For(Times.As(50))
                .WithSet(group => group.Create()
                    .WithStep(_engineFeature, (engine, ct) => engine.Start(ct), Milliseconds.As(1200))
                    .WithStep(_engineFeature, (engine, ct) => engine.Stop(ct), Milliseconds.As(800)))
                .Run();

        [Fact]
        public Task EngineShouldStartAndStopDuringTime()
            => UseFeatureSetGroup().During(Seconds.As(30))
                .WithSet(group => group.Create()
                    .WithStep(_engineFeature, (engine, ct) => engine.Start(ct), Milliseconds.As(1200))
                    .WithStep(_engineFeature, (engine, ct) => engine.Stop(ct), Milliseconds.As(800)))
                .Run();


        [Fact]
        public Task EngineShouldStartRevAndStop()
            => UseFeatureSetGroup().For(Times.As(50))
                .WithSet(group => group.Create()
                    .WithStep(_engineFeature, (engine, ct) => engine.Start(ct), Milliseconds.As(1200))
                    .WithStep(_engineFeature, (engine, ct) => engine.Rev3000(ct))
                    .WithStep(_engineFeature, (engine, ct) => engine.Stop(ct), Milliseconds.As(800)))
                .Run();

        [Fact]
        public Task EngineShouldStartRevAndStopDuringTime()
            => UseFeatureSetGroup().During(Minutes.As(1))
                .WithSet(group => group.Create().During(Seconds.As(20))
                    .WithStep(_engineFeature, (engine, ct) => engine.Start(ct))
                    .WithStep(_engineFeature, (engine, ct) => engine.Rev3000(ct))
                    .WithStep(_engineFeature, (engine, ct) => engine.Stop(ct)))
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