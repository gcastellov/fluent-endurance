using System;
using System.Threading;
using System.Threading.Tasks;
using FluentEndurance.Notifications;
using MediatR;

namespace FluentEndurance.Samples.NotificationHandlers
{
    public class StatusNotificationHandler : INotificationHandler<StatusNotification>, INotificationHandler<PerformanceStatusNotification>
    {
        public Action<string> Write { get; set; }

        public Task Handle(StatusNotification notification, CancellationToken cancellationToken)
        {
            this.Write?.Invoke(notification.Content);
            return Task.CompletedTask;
        }

        public Task Handle(PerformanceStatusNotification notification, CancellationToken cancellationToken)
        {
            this.Write?.Invoke(notification.Content);
            return Task.CompletedTask;
        }
    }
}