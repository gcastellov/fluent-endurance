using FluentAssertions;
using Xunit;

namespace FluentEndurance.Tests
{
    public class TimeTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(11)]
        [InlineData(111)]
        [InlineData(1111)]
        [InlineData(11111)]
        [InlineData(111111)]
        public void IntegerAsMilliseconds_ReturnsProperTimeSpan(int ms)
        {
            var actual = Milliseconds.As(ms);

            actual.Value.TotalMilliseconds.Should().Be(ms);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(22)]
        [InlineData(222)]
        [InlineData(2222)]
        [InlineData(22222)]
        [InlineData(222222)]
        public void IntegerAsSeconds_ReturnsProperTimeSpan(int seconds)
        {
            var actual = Seconds.As(seconds);

            actual.Value.TotalSeconds.Should().Be(seconds);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(33)]
        [InlineData(333)]
        [InlineData(3333)]
        [InlineData(33333)]
        [InlineData(333333)]
        public void IntegerAsMinutes_ReturnsProperTimeSpan(int minutes)
        {
            var actual = Minutes.As(minutes);

            actual.Value.TotalMinutes.Should().Be(minutes);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(44)]
        [InlineData(444)]
        [InlineData(4444)]
        [InlineData(44444)]
        [InlineData(444444)]
        public void IntegerAsHours_ReturnsProperTimeSpan(int hours)
        {
            var actual = Hours.As(hours);

            actual.Value.TotalHours.Should().Be(hours);
        }
    }
}