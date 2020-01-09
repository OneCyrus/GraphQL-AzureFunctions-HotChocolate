using HotChocolate.AspNetCore;
using HotChocolate.Execution;
using HotChocolate.Language;

namespace HotChocolate.AzureFunctions
{
    public interface IFunctionsGraphQLInjector
    {
        IAzureFunctionsMiddlewareOptions AzureFunctionsOptions { get; }
        IDocumentCache DocumentCache { get; }
        IDocumentHashProvider DocumentHashProvider { get; }
        IQueryExecutor Executor { get; }
    }
}