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
    public static class Function1
    {
        [FunctionName("graphql")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log,
            [Inject(typeof(IQueryExecutor))] IQueryExecutor executor)
        {
            var graphQLRequest = JsonConvert.DeserializeObject<GraphQLRequest>(await req.ReadAsStringAsync());

            log.LogInformation(graphQLRequest.Query);

            var result = await executor.ExecuteAsync(new QueryRequest(graphQLRequest.Query));

            return new OkObjectResult(result);
        }
    }
}
