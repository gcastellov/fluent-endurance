using System;
using MediatR;

namespace FluentEndurance.Notifications
{
    public class StatusNotification : INotification
    {
        public StatusNotification()
        {
            At = DateTime.UtcNow;
        }

        public StatusNotification(string content)
            : this()
        {
            Content = content;
        }

        public string Content { get; internal set; }

        public DateTime At { get; internal set; }
    }
}
