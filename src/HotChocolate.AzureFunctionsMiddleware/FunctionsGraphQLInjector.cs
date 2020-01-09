using HotChocolate.AspNetCore;
using HotChocolate.Execution;
using HotChocolate.Language;

namespace HotChocolate.AzureFunctions
{
    public class FunctionsGraphQLInjector : IFunctionsGraphQLInjector
    {
        public IQueryExecutor Executor { get; }
        public IDocumentCache DocumentCache { get; }
        public IDocumentHashProvider DocumentHashProvider { get; }
        public IAzureFunctionsMiddlewareOptions AzureFunctionsOptions { get; }

        public FunctionsGraphQLInjector(IQueryExecutor executor, IDocumentCache documentCache,
            IDocumentHashProvider documentHashProvider, IAzureFunctionsMiddlewareOptions azureFunctionsOptions)
        {
            Executor = executor;
            DocumentCache = documentCache;
            DocumentHashProvider = documentHashProvider;
            AzureFunctionsOptions = azureFunctionsOptions;
        }
    }
}
