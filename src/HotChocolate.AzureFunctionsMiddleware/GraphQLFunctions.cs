using HotChocolate.AspNetCore;
using HotChocolate.Execution;
using HotChocolate.Language;
using HotChocolate.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
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
        private readonly RequestHelper _requestHelper;
        private readonly JsonQueryResultSerializer _jsonQueryResultSerializer;

        public GraphQLFunctions(IQueryExecutor executor, IDocumentCache documentCache,
            IDocumentHashProvider documentHashProvider, IAzureFunctionsMiddlewareOptions azureFunctionsOptions /*JsonQueryResultSerializer jsonQueryResultSerializer*/)
        {
            Executor = executor;
            DocumentCache = documentCache;
            DocumentHashProvider = documentHashProvider;
            AzureFunctionsOptions = azureFunctionsOptions;

            _jsonQueryResultSerializer = new JsonQueryResultSerializer();

            _requestHelper = new RequestHelper(
              DocumentCache,
              DocumentHashProvider,
              AzureFunctionsOptions.MaxRequestSize,
              AzureFunctionsOptions.ParserOptions);
        }

        public async Task<IActionResult> ExecuteFunctionsQueryAsync(
            HttpContext context,
            CancellationToken cancellationToken)
        {
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

            var result = await Executor.ExecuteAsync(builder.Create());
            await _jsonQueryResultSerializer.SerializeAsync((IReadOnlyQueryResult)result, context.Response.Body);

            return new EmptyResult();
        }
    }
}
