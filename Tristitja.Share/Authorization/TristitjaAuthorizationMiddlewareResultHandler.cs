using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Tristitja.Share.Authorization;

public class TristitjaAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();

    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        var reason = authorizeResult.AuthorizationFailure?.FailureReasons
                .OfType<RouteGuardAuthorizationFailureReason>()
                .FirstOrDefault();

        if (reason is not null)
        {
            context.Response.StatusCode = StatusCodes.Status302Found;
            context.Response.Headers.Location = reason.RedirectUrl;
            return;
        }
        
        await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }
}
