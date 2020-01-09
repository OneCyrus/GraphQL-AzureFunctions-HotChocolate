using HotChocolate.AspNetCore;
using HotChocolate.Execution;
using HotChocolate.Language;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HotChocolate.AzureFunctions
{
    public interface IGraphQLFunctions
    {
        IAzureFunctionsMiddlewareOptions AzureFunctionsOptions { get; }
        IDocumentCache DocumentCache { get; }
        IDocumentHashProvider DocumentHashProvider { get; }
        IQueryExecutor Executor { get; }
        Task<IExecutionResult> ExecuteFunctionsQueryAsync(
            HttpContext context,
            CancellationToken cancellationToken);
    }
}