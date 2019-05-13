using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using HotChocolate.Execution;
using GraphQLAzureFunctions;

namespace GraphQLAzureFunctions
{
    public class GraphQL
    {
        private readonly IQueryExecutor _executor;

        public GraphQL(IQueryExecutor executor)
        {
            _executor = executor;
        }

        [FunctionName("graphql")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var graphQLRequest = JsonConvert.DeserializeObject<GraphQLRequest>(await req.ReadAsStringAsync());

            log.LogInformation(graphQLRequest.Query);

            var result = await _executor.ExecuteAsync(new QueryRequest(graphQLRequest.Query));

            return new OkObjectResult(result);
        }
    }
}
