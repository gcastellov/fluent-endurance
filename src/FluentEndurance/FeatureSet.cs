using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentEndurance.Notifications;
using MediatR;

namespace FluentEndurance
{
    public class FeatureSet
    {
        private readonly List<Step> _steps;
        private readonly IMediator _mediator;
        private string _name;
        private int _times;
        private TimeSpan _during;

        public FeatureSet(IMediator mediator)
        {
            _mediator = mediator;
            _steps = new List<Step>();
            _times = Times.Once.Value;
            _during = TimeSpan.Zero;
        }

        public FeatureSet For(Times times)
        {
            _times = times.Value;
            return this;
        }

        public FeatureSet During(Time during)
        {
            _during = during.Value;
            return this;
        }

        public FeatureSet As(string name)
        {
            _name = name;
            return this;
        }

        public FeatureSet WithStep<T>(T feature, Expression<Func<T, CancellationToken, Task>> useCase) where T : Feature
        {
            return WithStep(feature, useCase, Timeout.Being(int.MaxValue));
        }

        public FeatureSet WithStep<T>(T feature, Expression<Func<T, CancellationToken, Task>> useCase, Timeout timeout) where T : Feature
        {
            var rule = new Rule<T>().BindRuleTo(useCase);
            _steps.Add(new Step(feature, rule, timeout));
            return this;
        }

        internal Task Run()
        {
            return _during == TimeSpan.Zero
                ? RunUsingTimes()
                : RunUsingTimeSpan();
        }

        private async Task RunUsingTimeSpan()
        {
            var until = DateTime.UtcNow.Add(_during);

            while (DateTime.UtcNow < until)
            {
                await _mediator.Publish(new StatusNotification($"Executing {_name} at {DateTime.UtcNow} times"));

                await Execute();
            }
        }

        private async Task RunUsingTimes()
        {
            for (var i = 1; i <= _times; i++)
            {
                await _mediator.Publish(new StatusNotification($"Executing {_name} set for {i} times"));

                await Execute();
            }
        }

        private async Task Execute()
        {
            var setStopWatch = new Stopwatch();
            setStopWatch.Start();

            foreach (var step in _steps)
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                await step.Execute();

                stopWatch.Stop();

                var notification = new PerformanceStatusNotification(step.Name, stopWatch.Elapsed);
                step.AddNotification(notification);
                await _mediator.Publish(notification);
            }

            setStopWatch.Stop();

            await _mediator.Publish(new PerformanceStatusNotification(_name, setStopWatch.Elapsed));
        }
    }
}