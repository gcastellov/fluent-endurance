using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FluentEndurance
{
    public abstract class BaseTest
    {
        private readonly IHost _host;

        protected BaseTest(Action<IServiceCollection> configureServices)
        {
            var hostBuilder = new HostBuilder();

            hostBuilder.ConfigureServices(collection =>
            {
                collection.AddScoped<FeatureSetGroup>();
                collection.AddSingleton<IMediator>(sp => new Mediator(sp.GetService));
                configureServices(collection);
            });

            _host = hostBuilder.Build();
        }

        protected IHost Host => _host;

        protected FeatureSetGroup UseFeatureSetGroup(Times times)
        {
            return _host.Services.GetService<FeatureSetGroup>().For(times);
        }

        protected FeatureSetGroup UseFeatureSetGroup(Time during)
        {
            return _host.Services.GetService<FeatureSetGroup>().During(during);
        }
    }
}