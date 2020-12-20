using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Jboss.AspNetCore.Authentication.Keycloak.PolicyRequirements.ResourceAccess
{
    public class ResourceAccessRequirement : AuthorizationHandler<ResourceAccessRequirement>, IAuthorizationRequirement
    {
        private const string CURRENT_RESOURCE_PLACEHOLDER = "%CURRENT_RESOURCE%";
        internal const string CLAIM_TYPE = "resource_access";
        internal const string CLAIM_VALUE_TYPE = "JSON";

        // <static> because a constructor invokes one time in any service lifetimes
        private static ClientInstallation _installation;

        public ResourceAccessRequirement(ClientInstallation installation)
        {
            _installation = installation;
        }


        public string Resource { get; }
        public IReadOnlyCollection<string> Roles { get; }

        internal bool IsCurrentResource => Resource == CURRENT_RESOURCE_PLACEHOLDER;

        public ResourceAccessRequirement(params string[] roles)
            : this(CURRENT_RESOURCE_PLACEHOLDER, roles)
        {
        }

        public ResourceAccessRequirement(string resource, params string[] roles)
        {
            Resource = resource;
            Roles = roles;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            ResourceAccessRequirement requirement)
        {
            var claim = context.User.Claims.SingleOrDefault(x =>
                x.Type.Equals(CLAIM_TYPE, StringComparison.OrdinalIgnoreCase)
                && x.ValueType.Equals(CLAIM_VALUE_TYPE, StringComparison.OrdinalIgnoreCase));
            if (claim == null || string.IsNullOrEmpty(claim.Value))
            {
                return Task.CompletedTask;
            }

            var resourcesAccess = JsonSerializer.Deserialize<ResourceAccessCollection>(claim.Value);
            var clientId = requirement.IsCurrentResource
                ? _installation.Resource
                : requirement.Resource;
            if (!resourcesAccess.ContainsKey(clientId))
            {
                return Task.CompletedTask;
            }

            var resourceAccess = resourcesAccess[clientId];
            if (resourceAccess.Roles.Intersect(requirement.Roles).Any())
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }


        public override string ToString()
        {
            var clientId = IsCurrentResource ? _installation.Resource : Resource;
            var value = $"and Roles are one of the following values: ({string.Join("|", Roles)}) for client '{clientId}'.";
            return $"{nameof(ResourceAccessRequirement)}:Claim.Type={CLAIM_TYPE} {value}";
        }
    }
}
