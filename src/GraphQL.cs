using HotChocolate.AspNetCore;
using HotChocolate.AzureFunctions;
using HotChocolate.Execution;
using HotChocolate.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace GraphQLAzureFunctions
{
    public class GraphQL
    {
        private readonly IFunctionsGraphQLInjector _graphQLExecutor;

        public GraphQL(IFunctionsGraphQLInjector functionsGraphqlInjector)
        {
            _graphQLExecutor = functionsGraphqlInjector;
        }

        [FunctionName("graphql")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log, CancellationToken cancellationToken)
        {
            IExecutionResult result = await _graphQLExecutor.Executor.ExecuteFunctionsQueryAsync(
                _graphQLExecutor,
                req.HttpContext,
                cancellationToken);

            return new OkObjectResult(result);
        }
    }
}
