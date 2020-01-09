using HotChocolate.Execution;
using HotChocolate.Language;
using HotChocolate.AzureFunctions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HotChocolate.Server;
using System.Threading;

namespace HotChocolate.AspNetCore
{
    public static class AzureFunctionsMiddlewareExtension
    {
        public static IServiceCollection AddAzureFunctionsGraphQL(
            this IServiceCollection serviceCollection,
            IAzureFunctionsMiddlewareOptions options)
        {
            serviceCollection.AddSingleton<IAzureFunctionsMiddlewareOptions,
                AzureFunctionsMiddlewareOptions>();

            serviceCollection.AddTransient<IFunctionsGraphQLInjector, FunctionsGraphQLInjector>();

            return serviceCollection;
        }

        public static async Task<IExecutionResult> ExecuteFunctionsQueryAsync(
            this IQueryExecutor executor,
            IFunctionsGraphQLInjector functionsGraphqlInjector,
            HttpContext context,
            CancellationToken cancellationToken)
        {
            var _requestHelper = new RequestHelper(
               functionsGraphqlInjector.DocumentCache,
               functionsGraphqlInjector.DocumentHashProvider,
               functionsGraphqlInjector.AzureFunctionsOptions.MaxRequestSize,
               functionsGraphqlInjector.AzureFunctionsOptions.ParserOptions);

            using var stream = context.Request.Body;

            var requestQuery = await _requestHelper
                .ReadJsonRequestAsync(stream, cancellationToken)
                .ConfigureAwait(false);

            var builder = QueryRequestBuilder.New();

            if (requestQuery.Count > 0)
            {
                var firstQuery = requestQuery[0];

                builder
                    .SetQuery(firstQuery.Query)
                    .SetOperation(firstQuery.OperationName)
                    .SetQueryName(firstQuery.QueryName);

                if (firstQuery.Variables != null
                    && firstQuery.Variables.Count > 0)
                {
                    builder.SetVariableValues(firstQuery.Variables);
                }
            }

            return await executor.ExecuteAsync(builder.Create());
        }
    }
}
