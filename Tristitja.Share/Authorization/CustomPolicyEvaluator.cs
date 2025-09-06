using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Tristitja.Share.Authorization;

public sealed class CustomPolicyEvaluator : PolicyEvaluator
{
    private readonly IAuthorizationService _authorizationService;
    
    public CustomPolicyEvaluator(IAuthorizationService authorization) : base(authorization)
    {
        _authorizationService = authorization;
    }

    public override async Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy, AuthenticateResult authenticationResult, HttpContext context,
        object? resource)
    {
        ArgumentNullException.ThrowIfNull(policy);

        var result = await _authorizationService.AuthorizeAsync(context.User, resource, policy);
        if (result.Succeeded)
        {
            return PolicyAuthorizationResult.Success();
        }

        // If authentication was successful, return forbidden, otherwise challenge
        return (authenticationResult.Succeeded)
            ? PolicyAuthorizationResult.Forbid(result.Failure)
            : PolicyAuthorizationResult.Challenge(result.Failure);
    }
}
