using Microsoft.AspNetCore.Authorization;
using Tristitja.Share.Authorization.Handlers;

namespace Tristitja.Share.Authorization.Requirements;

public class UserNotLoggedInHandler : AuthorizationHandler<UserNotLoggedInRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserNotLoggedInRequirement requirement)
    {
        var authenticated= context.User.Identity?.IsAuthenticated ?? false;
        
        if (authenticated)
        {
            context.Fail(new RouteGuardAuthorizationFailureReason(this, "/"));
        }
        else
        {
            context.Succeed(requirement);
        }
        
        return Task.CompletedTask;
    }
}
