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

        public FeatureSetGroup(IMediator mediator)
        {
            _mediator = mediator;
            _sets = new List<FeatureSet>();
            _times = Times.Once.Value;
        }

        public FeatureSetGroup For(Times times)
        {
            _times = times.Value;
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

        public async Task Run()
        {
            for (var i = 0; i < _times; i++)
            {
                await _mediator.Publish(new StatusNotification($"Running group for {i} times"));

                foreach (var featureSet in _sets)
                {
                    await featureSet.Run();
                }
            }
        }
    }
}