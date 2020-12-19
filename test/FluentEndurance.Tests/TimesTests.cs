using FluentAssertions;
using Xunit;

namespace FluentEndurance.Tests
{
    public class TimesTests
    {
        [Fact]
        public void Once_IsOneRepetition()
        {
            var actual = Times.Once;

            actual.Value.Should().Be(1);
        }

        [Fact]
        public void Twice_IsTwoRepetitions()
        {
            var actual = Times.Twice;

            actual.Value.Should().Be(2);
        }

        [Fact]
        public void Max_IsIntegerMax()
        {
            var actual = Times.Max;

            actual.Value.Should().Be(int.MaxValue);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(22)]
        [InlineData(222)]
        [InlineData(2222)]
        [InlineData(22222)]
        public void Other_IsProperRepetition(int other)
        {
            var actual = Times.As(other);

            actual.Value.Should().Be(other);
        }
    }
}