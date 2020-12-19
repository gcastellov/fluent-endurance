using System.Threading;
using System.Threading.Tasks;
using FluentEndurance.Notifications;
using MediatR;
using Moq;
using Xunit;

namespace FluentEndurance.Tests
{
    public class FeatureSetTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly FeatureSet _featureSet;

        public FeatureSetTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _featureSet = new FeatureSet(_mediatorMock.Object);
        }

        [Fact]
        public async Task SetDefinedWithName_WhenRun_NotificationsContainName()
        {
            const string setName = "SetName";
            _featureSet.As(setName);

            await _featureSet.Run();

            _mediatorMock.Verify(
                m => m.Publish(It.Is<StatusNotification>(n => n.Content.Contains(setName)), It.IsAny<CancellationToken>()), 
                Moq.Times.AtLeastOnce);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task SetDefinedToRepeat_WhenRun_Loops(int times)
        {
            _featureSet.For(Times.As(times));

            await _featureSet.Run();

            _mediatorMock.Verify(
                m => m.Publish(It.IsAny<PerformanceStatusNotification>(), It.IsAny<CancellationToken>()), 
                Moq.Times.Exactly(times));
        }

        [Fact]
        public async Task SetDefinedToRepeatDuringTime_WhenRun_Loops()
        {
            _featureSet.During(Seconds.As(5));

            await _featureSet.Run();

            _mediatorMock.Verify(
                m => m.Publish(It.IsAny<PerformanceStatusNotification>(), It.IsAny<CancellationToken>()),
                Moq.Times.AtLeast(2));
        }

        [Fact]
        public async Task SetDefinedWithSteps_WhenRun_StepsAreCalled()
        {
            var feature = new Mock<FeatureTest>();

            _featureSet.For(Times.Once);
            _featureSet.WithStep(feature.Object, (test, _) => test.Do(_));

            await _featureSet.Run();

            feature.Verify(m => m.Do(It.IsAny<CancellationToken>()), Moq.Times.Once);
        }
    }
}
