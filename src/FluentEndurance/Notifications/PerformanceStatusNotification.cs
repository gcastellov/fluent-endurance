using System;

namespace FluentEndurance.Notifications
{
    public class PerformanceStatusNotification : StatusNotification
    {
        public PerformanceStatusNotification(string name, TimeSpan elapsedTime)
            : base($"Executed {name} taking {elapsedTime.TotalMilliseconds} ms")
        {
            Name = name;
            ElapsedTime = elapsedTime;
        }

        public string Name { get; }

        public TimeSpan ElapsedTime { get; }
    }
}