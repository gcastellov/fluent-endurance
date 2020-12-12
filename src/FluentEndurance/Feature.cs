using System.Collections.Generic;
using FluentEndurance.Notifications;

namespace FluentEndurance
{
    public abstract class Feature
    {
        private readonly List<PerformanceStatusNotification> _notifications;

        protected Feature()
        {
            _notifications = new List<PerformanceStatusNotification>();
        }

        public IEnumerable<PerformanceStatusNotification> Notifications => _notifications;

        internal void Add(PerformanceStatusNotification notification)
        {
            _notifications.Add(notification);
        }
    }
}