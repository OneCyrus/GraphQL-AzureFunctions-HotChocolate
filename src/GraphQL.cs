using HotChocolate.Execution;
using HotChocolate.Language;
using HotChocolate.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace GraphQLAzureFunctions
{
    public class GraphQL
    {
        private readonly IDocumentCache _documentCache;
        private readonly IDocumentHashProvider _documentHashProvider;
        private readonly IQueryExecutor _executor;

        public GraphQL(IQueryExecutor executor, IDocumentCache documentCache,
            IDocumentHashProvider documentHashProvider)
        {
            _executor = executor;
            _documentCache = documentCache;
            _documentHashProvider = documentHashProvider;
        }

        [FunctionName("graphql")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log, CancellationToken cancellationToken)
        {
            var maxRequestSize = 20 * 1000 * 1000;

            var _requestHelper = new RequestHelper(
               _documentCache,
               _documentHashProvider,
               maxRequestSize,
               new ParserOptions());

            var builder = QueryRequestBuilder.New();

            using (Stream stream = req.HttpContext.Request.Body)
            {
                var requestQuery = await _requestHelper
                    .ReadJsonRequestAsync(stream, cancellationToken)
                    .ConfigureAwait(false);

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
                else
                {
                    return new NotFoundObjectResult(null);
                }
            }

            var result = await _executor.ExecuteAsync(builder.Create());

            return new OkObjectResult(result);
        }
    }
}
