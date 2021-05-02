using LocalParks.Configuration.Middleware;
using Microsoft.AspNetCore.Builder;

namespace LocalParks.Configuration.Injection
{
    public static class QueryStringMiddlewareExtensions
    {
        public static IApplicationBuilder UseQueryStringMiddleware(this IApplicationBuilder builder, QueryStringMiddlewareOptions configureOptions)
        {
            return builder.UseMiddleware<QueryStringEncryptionMiddleware>(
                configureOptions.EncryptionKey,
                configureOptions.ParameterValue);
        }
    }
}

