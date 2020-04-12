using HotChocolate.AzureFunctions;
using Microsoft.Extensions.DependencyInjection;

namespace HotChocolate.AspNetCore
{
    public static class AzureFunctionsMiddlewareExtension
    {
        public static IServiceCollection AddAzureFunctionsGraphQL(
            this IServiceCollection serviceCollection,
            IAzureFunctionsMiddlewareOptions options)
        {
            serviceCollection.AddSingleton<IAzureFunctionsMiddlewareOptions>(options);

            serviceCollection.AddSingleton<IGraphQLFunctions, GraphQLFunctions>();

            return serviceCollection;
        }

        public static IServiceCollection AddAzureFunctionsGraphQL(
            this IServiceCollection serviceCollection)
        {
            serviceCollection.AddAzureFunctionsGraphQL(new AzureFunctionsMiddlewareOptions());

            return serviceCollection;
        }
    }
}
