using System.Net.Http.Headers;

namespace WorkTrace.WebApp.Services.ApiServices;

public class AuthTokenHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _contextAccessor;

    public AuthTokenHandler(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = _contextAccessor.HttpContext?.Session.GetString("AuthToken");

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
