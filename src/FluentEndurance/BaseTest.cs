using System;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FluentEndurance
{
    public abstract class BaseTest
    {
        private readonly IHost _host;
        private IConfigurationRoot _configuration;

        protected BaseTest(Action<IConfigurationBuilder> configureApp,  Action<IServiceCollection> configureServices)
        {
            var hostBuilder = new HostBuilder();
            hostBuilder.ConfigureAppConfiguration(builder =>
            {
                configureApp(builder);
                _configuration = builder.Build();
            });

            hostBuilder.ConfigureServices(collection =>
            {
                collection.AddScoped<FeatureSetGroup>();
                collection.AddSingleton<IMediator>(sp => new Mediator(sp.GetService));
                configureServices(collection);
            });

            _host = hostBuilder.Build();
        }

        protected IHost Host => _host;

        protected IConfigurationRoot Configuration => _configuration;

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