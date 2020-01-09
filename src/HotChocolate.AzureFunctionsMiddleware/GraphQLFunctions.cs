using HotChocolate.AspNetCore;
using HotChocolate.Execution;
using HotChocolate.Language;
using HotChocolate.Server;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HotChocolate.AzureFunctions
{
    public class GraphQLFunctions : IGraphQLFunctions
    {
        public IQueryExecutor Executor { get; }
        public IDocumentCache DocumentCache { get; }
        public IDocumentHashProvider DocumentHashProvider { get; }
        public IAzureFunctionsMiddlewareOptions AzureFunctionsOptions { get; }

        public GraphQLFunctions(IQueryExecutor executor, IDocumentCache documentCache,
            IDocumentHashProvider documentHashProvider, IAzureFunctionsMiddlewareOptions azureFunctionsOptions)
        {
            Executor = executor;
            DocumentCache = documentCache;
            DocumentHashProvider = documentHashProvider;
            AzureFunctionsOptions = azureFunctionsOptions;
        }

        public async Task<IExecutionResult> ExecuteFunctionsQueryAsync(
            HttpContext context,
            CancellationToken cancellationToken)
        {
            var _requestHelper = new RequestHelper(
               DocumentCache,
               DocumentHashProvider,
               AzureFunctionsOptions.MaxRequestSize,
               AzureFunctionsOptions.ParserOptions);

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

            return await Executor.ExecuteAsync(builder.Create());
        }
    }
}
