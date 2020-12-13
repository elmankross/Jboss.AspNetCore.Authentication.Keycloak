using System;
using System.Collections.Generic;

namespace AspNetCore.KeycloakAuthentication.PolicyRequirements.ResourceAccess
{
    public class ResourceAccessCollection : Dictionary<string, ResourceAccess>
    {
        public ResourceAccessCollection()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }
    }
}
