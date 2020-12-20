using System;
using System.Collections.Generic;

namespace Jboss.AspNetCore.Authentication.Keycloak.PolicyRequirements.ResourceAccess
{
    public class ResourceAccessCollection : Dictionary<string, ResourceAccess>
    {
        public ResourceAccessCollection()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }
    }
}
