using HotChocolate;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.DependencyInjection;
using StarWars;
using StarWars.Data;
using StarWars.Types;
using System;

namespace GraphQLAzureFunctions
{
    public class InjectConfiguration : IExtensionConfigProvider
    {
        private IServiceProvider _serviceProvider;

        public void Initialize(ExtensionConfigContext context)
        {
            var services = new ServiceCollection();
            RegisterServices(services);
            _serviceProvider = services.BuildServiceProvider(true);

            context
                .AddBindingRule<InjectAttribute>()
                .BindToInput<dynamic>(i => _serviceProvider.GetRequiredService(i.Type));
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<CharacterRepository>();
            services.AddSingleton<ReviewRepository>();

            services.AddSingleton<Query>();

            services.AddGraphQL(sp => Schema.Create(c =>
            {
                c.RegisterServiceProvider(sp);

                c.RegisterQueryType<QueryType>();

                c.RegisterType<HumanType>();
                c.RegisterType<DroidType>();
                c.RegisterType<EpisodeType>();
            }));
        }
    }
}
