using System;

namespace Microsoft.AspNetCore.Authentication.Keycloak
{
    public class ClientInstallation
    {
        /// <summary>
        /// Default client's installation's file
        /// </summary>
        internal const string FILE = "keycloak.json";

        public string Realm { get; set; } = "master";
        public Uri AuthServerUrl { get; set; } = new Uri("http://localhost:8080/auth/");
        public string Resource { get; set; }
        public bool VerifyTokenAudience { get; set; }
        public KeycloakClientInstallationCredentials Credentials { get; set; }

        /// <summary>
        /// <seealso cref="/.well-known/openid-configuration"/>
        /// </summary>
        private Uri _issuer;
        public Uri Issuer
        {
            get
            {
                if (_issuer == null)
                {
                    var b = new UriBuilder(AuthServerUrl);
                    b.Path += "realms/" + Realm;
                    _issuer = b.Uri;
                }
                return _issuer;
            }
        }

        /// <summary>
        /// <seealso cref="/.well-known/openid-configuration"/>
        /// </summary>
        private Uri _tokenEndpoint;
        public Uri TokenEndpoint
        {
            get
            {
                if (_tokenEndpoint == null)
                {
                    var b = new UriBuilder(Issuer);
                    b.Path += "/protocol/openid-connect/token";
                    _tokenEndpoint = b.Uri;
                }
                return _tokenEndpoint;
            }
        }


        public ClientInstallation()
        {
            Credentials = new KeycloakClientInstallationCredentials();
        }
    }


    public class KeycloakClientInstallationCredentials
    {
        public string Secret { get; set; }
    }
}
