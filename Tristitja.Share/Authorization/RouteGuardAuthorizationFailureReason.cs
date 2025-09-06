using Microsoft.AspNetCore.Authorization;

namespace Tristitja.Share.Authorization;

public class RouteGuardAuthorizationFailureReason : AuthorizationFailureReason
{
    public RouteGuardAuthorizationFailureReason(IAuthorizationHandler handler, string redirectUrl)
        : base(handler, "whatever")
    {
        RedirectUrl = redirectUrl;
    }

    public string RedirectUrl { get; }
    
}
