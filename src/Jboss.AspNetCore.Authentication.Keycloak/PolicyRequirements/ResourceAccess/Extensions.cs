using Microsoft.AspNetCore.Authorization;

namespace Jboss.AspNetCore.Authentication.Keycloak
{
    using PolicyRequirements.ResourceAccess;

    public static class ResourceAccessExtensions
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
