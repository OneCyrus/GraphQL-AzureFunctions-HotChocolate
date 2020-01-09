namespace HotChocolate.AspNetCore
{
    public interface IAzureFunctionsMiddlewareOptions
        : IParserOptionsAccessor
    {
        int MaxRequestSize { get; }
    }
}
