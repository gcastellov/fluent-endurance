using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace FluentEndurance.Tests
{
    public class StepTests
    {
        private readonly Rule<FeatureTest> _rule;

        public StepTests()
        {
            _rule = new Rule<FeatureTest>().BindRuleTo((test, token) => test.Do(token));
        }

        [Fact]
        public async Task StepWithoutTimeout_WhenExecute_DoesNotUseCancellationToken()
        {
            var featureMock = new Mock<FeatureTest>();
            var step = new Step(featureMock.Object, _rule, Span.As(TimeSpan.MaxValue));

            await step.Execute();

            featureMock.Verify(m => m.Do(It.Is<CancellationToken>(token => token == CancellationToken.None)));
        }

        [Fact]
        public async Task StepWithTimeout_WhenExecute_UsesCancellationToken()
        {
            var featureMock = new Mock<FeatureTest>();
            var step = new Step(featureMock.Object, _rule, Milliseconds.As(100));

            await step.Execute();

            featureMock.Verify(m => m.Do(It.Is<CancellationToken>(token => token != CancellationToken.None)));
        }

        [Fact]
        public void StepWithTimeout_WhenExecute_RaisesExceptionIfTimeExceeds()
        {
            var feature = new FeatureTest();
            var step = new Step(feature, _rule, Milliseconds.As(100));

            Func<Task> action = async () => await step.Execute();

            action.Should().ThrowAsync<OperationCanceledException>();
        }
    }
}