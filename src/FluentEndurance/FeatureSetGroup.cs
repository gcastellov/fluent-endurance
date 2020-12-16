using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentEndurance.Notifications;
using MediatR;

namespace FluentEndurance
{
    public class FeatureSetGroup
    {
        private readonly List<FeatureSet> _sets;
        private readonly IMediator _mediator;
        private int _times;
        private TimeSpan _during;

        public FeatureSetGroup(IMediator mediator)
        {
            _mediator = mediator;
            _sets = new List<FeatureSet>();
            _times = Times.Once.Value;
            _during = TimeSpan.Zero;
        }

        public FeatureSetGroup For(Times times)
        {
            _times = times.Value;
            return this;
        }

        public FeatureSetGroup During(Time during)
        {
            _during = during.Value;
            return this;
        }

        public FeatureSetGroup WithSet(Func<FeatureSetGroup, FeatureSet> createFeatureSet)
        {
            return WithSet(createFeatureSet(this));
        }

        public FeatureSetGroup WithSet(FeatureSet set)
        {
            _sets.Add(set);
            return this;
        }

        public FeatureSet Create()
        {
            return new FeatureSet(this._mediator);
        }

        public Task Run()
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
                await _mediator.Publish(new StatusNotification($"Running group at {DateTime.UtcNow} times"));

                await Execute();
            }
        }


        private async Task RunUsingTimes()
        {
            for (var i = 1; i <= _times; i++)
            {
                await _mediator.Publish(new StatusNotification($"Running group for {i} times"));

                await Execute();
            }
        }

        private async Task Execute()
        {
            foreach (var featureSet in _sets)
            {
                await featureSet.Run();
            }
        }
    }
}