using System;
using System.Threading;
using System.Threading.Tasks;
using FluentEndurance.Notifications;

namespace FluentEndurance
{
    public class Step
    {
        private readonly IRule _rule;
        private readonly TimeSpan _timeout;
        private readonly Feature _feature;

        public Step(Feature feature, IRule rule, Time timeout)
        {
            _feature = feature;
            _rule = rule;
            _timeout = timeout.Value;
        }

        public string Name => _rule.Name;

        internal async Task Execute()
        {
            var cancellationToken = CancellationToken.None;
            CancellationTokenSource cancellationTokenSource = null;
            if (_timeout != TimeSpan.MaxValue)
            {
                cancellationTokenSource = new CancellationTokenSource(_timeout);
                cancellationToken = cancellationTokenSource.Token;
            }

            var task = (Task)_rule.GetMethodInfo().Invoke(_feature, new object[] {cancellationToken });
            await Task.Run(async () => await task, cancellationToken);

            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Token.ThrowIfCancellationRequested();
                cancellationTokenSource.Dispose();
            }
        }

        internal void AddNotification(PerformanceStatusNotification notification)
        {
            _feature.Add(notification);
        }
    }
}