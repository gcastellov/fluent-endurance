using System;

namespace FluentEndurance
{
    public abstract class Time
    {
        protected Time(TimeSpan timeSpan)
        {
            Value = timeSpan;
        }

        internal TimeSpan Value { get; }
    }

    public class Span : Time
    {
        public static Time As(TimeSpan timeSpan) => new Span(timeSpan);

        private Span(TimeSpan timeSpan)
            : base(timeSpan)
        {
        }
    }

    public static class Milliseconds
    {
        public static Time As(int ms) => Span.As(new TimeSpan(0, 0, 0, ms));
    }

    public static class Seconds
    {
        public static Time As(int seconds) => Span.As(new TimeSpan(0, 0, seconds));
    }

    public static class Minutes
    {
        public static Time As(int minutes) => Span.As(new TimeSpan(0, minutes, 0));
    }

    public static class Hours
    {
        public static Time As(int hours) => Span.As(new TimeSpan(hours, 0, 0));
    }
}