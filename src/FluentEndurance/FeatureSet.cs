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

        public FeatureSet(IMediator mediator)
        {
            _mediator = mediator;
            _steps = new List<Step>();
            _times = Times.Once.Value;
        }

        public FeatureSet For(Times times)
        {
            _times = times.Value;
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

        internal async Task Run()
        {
            for (int i = 0; i < _times; i++)
            {
                await _mediator.Publish(new StatusNotification($"Executing {_name} set for {i} times"));

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
}