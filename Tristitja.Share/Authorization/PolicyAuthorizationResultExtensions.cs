using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Tristitja.Share.Authorization;

public static class PolicyAuthorizationResultExtensions
{
    extension(PolicyAuthorizationResult policyAuthorizationResult)
    {
        public static PolicyAuthorizationResult Challenge(AuthorizationFailure failure)
        {
            Type t = typeof(PolicyAuthorizationResult);
            
            var res = (PolicyAuthorizationResult) PolicyAuthorizaionResultCtor.Invoke([]);
            
            ChallengedProperty.SetValue(res, true);
            AuthorizationFailureProperty.SetValue(res, failure);

            return res;
        }
    }
    
    private static readonly ConstructorInfo PolicyAuthorizaionResultCtor = typeof(PolicyAuthorizationResult).GetConstructor(
        BindingFlags.Instance | BindingFlags.NonPublic,
        null, CallingConventions.HasThis, [], null)!;
    
    private static readonly PropertyInfo ChallengedProperty = typeof(PolicyAuthorizationResult)
        .GetProperty("Challenged", BindingFlags.Instance | BindingFlags.Public)!;
    
    private static readonly PropertyInfo AuthorizationFailureProperty = typeof(PolicyAuthorizationResult)
        .GetProperty("AuthorizationFailure", BindingFlags.Instance | BindingFlags.Public)!;
}
