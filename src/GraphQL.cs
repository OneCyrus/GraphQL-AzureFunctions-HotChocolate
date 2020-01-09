using HotChocolate.AzureFunctions;
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
        private readonly IGraphQLFunctions _graphQLFunctions;

        public GraphQL(IGraphQLFunctions functionsGraphqlInjector)
        {
            _graphQLFunctions = functionsGraphqlInjector;
        }

        [FunctionName("graphql")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log, CancellationToken cancellationToken)
        {
            var result = await _graphQLFunctions.ExecuteFunctionsQueryAsync(
                req.HttpContext,
                cancellationToken);

            return new OkObjectResult(result);
        }
    }
}
