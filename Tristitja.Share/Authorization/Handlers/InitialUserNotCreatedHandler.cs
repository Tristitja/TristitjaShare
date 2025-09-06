using Microsoft.AspNetCore.Authorization;
using Tristitja.Auth.Local.Services;
using Tristitja.Share.Authorization.Requirements;

namespace Tristitja.Share.Authorization.Handlers;

public class InitialUserNotCreatedHandler : AuthorizationHandler<InitialUserNotCreatedRequirement>
{
    private readonly IUserService _userService;

    public InitialUserNotCreatedHandler(IUserService userService)
    {
        _userService = userService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, InitialUserNotCreatedRequirement requirement)
    {
        var initialUserCreated = await _userService.GetInitialUser();

        if (initialUserCreated is not null)
        {
            context.Fail(new RouteGuardAuthorizationFailureReason(this, "/"));
        }
        else
        {
            context.Succeed(requirement);
        }
    }
}
