using AspNetCore.KeycloakAuthentication.PolicyRequirements.ResourceAccess;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCore.KeycloakAuthentication.PolicyRequirements
{
    public static class Extensions
    {
        /// <summary>
        /// Requires one from specified roles for CURRENT resource
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public static AuthorizationPolicyBuilder RequireResourceRole(this AuthorizationPolicyBuilder builder, string[] roles)
        {
            return builder.RequireClaim(ResourceAccessRequirement.CLAIM_TYPE)
                .AddRequirements(new ResourceAccessRequirement(roles));
        }


        /// <summary>
        /// Requires one from specified roles for the resource 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="resource"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public static AuthorizationPolicyBuilder RequireResourceRole(this AuthorizationPolicyBuilder builder, string resource, string[] roles)
        {
            return builder.RequireClaim(ResourceAccessRequirement.CLAIM_TYPE)
                .AddRequirements(new ResourceAccessRequirement(resource, roles));
        }
    }
}
