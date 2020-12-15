using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Microsoft.AspNetCore.Authentication.Keycloak.Providers.KeycloakConfiguration
{
    public class KeycloakConfigurationSource : JsonConfigurationSource
    {
        /// <summary>
        /// Builds the <see cref="JsonConfigurationProvider"/> for this source.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <returns>A <see cref="JsonConfigurationProvider"/></returns>
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            EnsureDefaults(builder);
            return new KeycloakConfigurationProvider(this);
        }
    }
}
