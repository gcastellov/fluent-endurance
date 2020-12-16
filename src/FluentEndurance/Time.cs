using System;

namespace FluentEndurance
{
    public class Time
    {
        public static Time Milliseconds(int ms) => new Time(new TimeSpan(0, 0, 0, ms));

        public static Time Seconds(int seconds) => new Time(new TimeSpan(0, 0, seconds));

        public static Time Minutes(int minutes) => new Time(new TimeSpan(0, minutes, 0));

        public static Time Hours(int hours) => new Time(new TimeSpan(hours, 0, 0));

        public static Time Span(TimeSpan timeSpan) => new Time(timeSpan);


        private Time(TimeSpan timeSpan)
        {
            Value = timeSpan;
        }

        internal TimeSpan Value { get; }
    }
}