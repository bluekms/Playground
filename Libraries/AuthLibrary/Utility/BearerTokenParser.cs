using System.Net.Http.Headers;
using System.Security.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace AuthLibrary.Utility;

internal sealed class BearerTokenParser
{
    private const string BearerHeaderSchema = "Bearer";

    public static string GetBearerToken(HttpRequest? request)
    {
        if (request == null)
        {
            throw new AuthenticationException($"{nameof(request)} is null");
        }

        var authorization = request.Headers[HeaderNames.Authorization];
        if (string.IsNullOrEmpty(authorization))
        {
            throw new AuthenticationException($"Not found {HeaderNames.Authorization}");
        }

        var headerValue = AuthenticationHeaderValue.Parse(authorization);
        if (headerValue.Scheme != BearerHeaderSchema)
        {
            throw new AuthenticationException($"{headerValue.Scheme} is not {BearerHeaderSchema}");
        }

        if (string.IsNullOrEmpty(headerValue.Parameter))
        {
            throw new AuthenticationException($"{BearerHeaderSchema} is null or empty");
        }

        return headerValue.Parameter;
    }
}
