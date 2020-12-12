using System.Threading;
using System.Threading.Tasks;
using FluentEndurance.Notifications;

namespace FluentEndurance
{
    public class Step
    {
        private readonly IRule _rule;
        private readonly int _timeout;
        private readonly Feature _feature;

        public Step(Feature feature, IRule rule, Timeout timeout)
        {
            _feature = feature;
            _rule = rule;
            _timeout = timeout.Value;
        }

        public string Name => _rule.Name;

        internal async Task Execute()
        {
            var cancellationTokenSource = new CancellationTokenSource(_timeout);
            var task = (Task)_rule.GetMethodInfo().Invoke(_feature, new object[] { cancellationTokenSource.Token });
            await Task.Run(async () => await task, cancellationTokenSource.Token);
            cancellationTokenSource.Token.ThrowIfCancellationRequested();
        }

        internal void AddNotification(PerformanceStatusNotification notification)
        {
            _feature.Add(notification);
        }
    }
}