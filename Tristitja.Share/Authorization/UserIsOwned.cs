using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Tristitja.Share.Authorization;

public class UserIsOwnedRequirement : IAuthorizationRequirement
{
    public required string UserId { get; init; }
}

public class UserIsOwnedHandler : AuthorizationHandler<UserIsOwnedRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserIsOwnedRequirement requirement)
    {
        if (requirement.UserId != context.User?.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value)
        {
            context.Fail();
        }
        else
        {
            context.Succeed(requirement);
        }
        
        return Task.CompletedTask;
    }
}
