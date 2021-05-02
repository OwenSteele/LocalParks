using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Threading.Tasks;

namespace LocalParks.Configuration.Middleware
{
    public class QueryStringEncryptionMiddleware
    {
        private readonly string _parameter;
        private static IHttpContextAccessor _httpContextAccessor;
        private readonly IDataProtector _protector;
        private readonly RequestDelegate _next;
        public QueryStringEncryptionMiddleware(string key, string parameter, IHttpContextAccessor httpContextAccessor, IDataProtectionProvider provider, RequestDelegate next)
        {
            _httpContextAccessor = httpContextAccessor;
            _protector = provider.CreateProtector(key);
            _next = next;
            _parameter = parameter;
        }
        public async Task Invoke(HttpContext context)
        {
            if (UriHelper.GetEncodedUrl(context.Request).Contains("?"))
            {
                string contextQuery = GetAbsoluteUri().Query.ToString();
                if (contextQuery.Contains(_parameter))
                {
                    var enc = context.Request.Query[_parameter];
                    enc = _protector.Unprotect(enc);
                    var queryString = new QueryString(enc);
                    context.Request.QueryString = queryString;
                }
                else if (context.Request.Method.Equals("GET"))
                {
                    if (!string.IsNullOrWhiteSpace(contextQuery))
                    {
                        string encryptedQuery = _protector.Protect(contextQuery);
                        string redirectToPagePath = $"{context.Request.Path.Value}?{_parameter}={encryptedQuery}";
                        context.Response.Redirect(redirectToPagePath);
                    }
                }
            }
            await _next.Invoke(context);
        }
        private static Uri GetAbsoluteUri()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var uriBuilder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Path = request.Path.ToString(),
                Query = request.QueryString.ToString()
            };
            return uriBuilder.Uri;
        }
    }
}

