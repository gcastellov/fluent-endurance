using System.Threading;
using System.Threading.Tasks;
using FluentEndurance.Notifications;
using MediatR;
using Moq;
using Xunit;

namespace FluentEndurance.Tests
{
    public class FeatureSetGroupTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly FeatureSetGroup _featureSetGroup;

        public FeatureSetGroupTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _featureSetGroup = new FeatureSetGroup(_mediatorMock.Object);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GroupDefinedToRepeat_WhenRun_Loops(int times)
        {
            _featureSetGroup.For(Times.As(times));

            await _featureSetGroup.Run();

            _mediatorMock.Verify(
                m => m.Publish(It.IsAny<StatusNotification>(), It.IsAny<CancellationToken>()), 
                Moq.Times.Exactly(times));
        }

        [Fact]
        public async Task GroupDefinedToRepeatDuringTime_WhenRun_Loops()
        {
            _featureSetGroup.During(Seconds.As(5));

            await _featureSetGroup.Run();

            _mediatorMock.Verify(
                m => m.Publish(It.IsAny<StatusNotification>(), It.IsAny<CancellationToken>()),
                Moq.Times.AtLeast(2));
        }

        [Fact]
        public async Task GroupDefinedWithSet_WhenRun_SetIsCalled()
        {
            _featureSetGroup.WithSet(group => group.Create());

            await _featureSetGroup.Run();

            _mediatorMock.Verify(
                m => m.Publish(It.Is<StatusNotification>(n => n.Content.Contains("group")), CancellationToken.None), 
                Moq.Times.Once);

            _mediatorMock.Verify(
                m => m.Publish(It.Is<StatusNotification>(n => n.Content.Contains("set")), CancellationToken.None),
                Moq.Times.Once);
        }
    }
}