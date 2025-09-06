using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Tristitja.Auth.Local.Dto;
using Tristitja.Auth.Local.Services;

namespace Tristitja.Share.Endpoints;

public static class AuthEndpointsIEndpointRouteBuilderExtension
{
    extension(IEndpointRouteBuilder endpoints)
    {
        public void MapAuthEndpoints()
        {
            endpoints.MapPost("/loginexec", async Task (
                HttpContext ctx,
                [FromBody] LoginRequest req,
                LoginRequest.LoginRequestValidator validator,
                IUserService userService,
                ISessionService sessionService) =>
            {
                var result = await validator.ValidateAsync(req);
                
                if (!result.IsValid)
                {
                    ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }
                
                var user = await userService.AuthenticateUser(req.Username, req.Password);

                if (user is null)
                {
                    ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }

                var session = await sessionService.CreateSessionAsync(user);
                var claimsPrincipal = sessionService.SessionToClaimsPrincipal(session);

                await ctx.SignInAsync(claimsPrincipal);
                ctx.Response.StatusCode = StatusCodes.Status204NoContent;
            });

            endpoints.MapDelete("/logoutexec", async Task (ctx) =>
            {
                await ctx.SignOutAsync();
                ctx.Response.StatusCode = StatusCodes.Status204NoContent;
            });
        }
    }
}
