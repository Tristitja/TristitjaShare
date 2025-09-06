using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Tristitja.Share.Authorization.Handlers;
using Tristitja.Share.Authorization.Requirements;

namespace Tristitja.Share.Authorization;

public static class AuthorizationServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public void AddTristitjaShareAuthorization()
        {
            services.AddScoped<IAuthorizationHandler, InitialUserNotCreatedHandler>();
            services.AddScoped<IAuthorizationHandler, UserNotLoggedInHandler>();
            
            services
                .AddSingleton<IAuthorizationMiddlewareResultHandler, TristitjaAuthorizationMiddlewareResultHandler>();

            services.AddTransient<IPolicyEvaluator, CustomPolicyEvaluator>();
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationConstants.InitialUserNotCreated, policy =>
                    policy.Requirements.Add(new InitialUserNotCreatedRequirement()));
                
                options.AddPolicy(AuthorizationConstants.UserNotLoggedIn, policy =>
                    policy.Requirements.Add(new UserNotLoggedInRequirement()));
            });
        }
    }
}
