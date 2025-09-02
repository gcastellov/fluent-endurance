using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FluentEndurance
{
    public abstract class BaseTest
    {
        private readonly IConfigurationRoot _configuration;
        private readonly IServiceProvider _serviceProvider;

        protected BaseTest(Action<IConfigurationBuilder> configureApp, Action<IServiceCollection> configureServices)
        {
            var configBuilder = new ConfigurationBuilder();
            var services = new ServiceCollection()
                .AddLogging()
                .AddScoped<FeatureSetGroup>()
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(this.GetType().Assembly));
            
            configureServices(services);
            configureApp(configBuilder);
            
            _configuration = configBuilder.Build();
            _serviceProvider = services.BuildServiceProvider();
        }

        protected IConfigurationRoot Configuration => _configuration;

        protected FeatureSetGroup UseFeatureSetGroup() => _serviceProvider.GetRequiredService<FeatureSetGroup>();
    }
}