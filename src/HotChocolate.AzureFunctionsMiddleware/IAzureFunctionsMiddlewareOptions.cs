namespace HotChocolate.AspNetCore
{
    public interface IAzureFunctionsMiddlewareOptions
        : IPathOptionAccessor
        , IParserOptionsAccessor
    {
        int MaxRequestSize { get; }
    }
}
