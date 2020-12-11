using System;
using System.Text.Json.Serialization;

namespace AspNetCore.KeycloakAuthentication
{
    public class KeycloakClientInstallation
    {
        /// <summary>
        /// Default client's installation's file
        /// </summary>
        internal const string FILE = "keycloak.json";


        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("realm")]
        public string Realm { get; set; } = "master";

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("auth-server-url")]
        public Uri ServerUrl { get; set; } = new Uri("http://localhost:8080/auth/");

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("resource")]
        public string Resource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("verify-token-audience")]
        public bool VerifyTokenAudience { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("credentials")]
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
                    var b = new UriBuilder(ServerUrl);
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


        public KeycloakClientInstallation()
        {
            Credentials = new KeycloakClientInstallationCredentials();
        }
    }


    public class KeycloakClientInstallationCredentials
    {
        [JsonPropertyName("secret")]
        public string Secret { get; set; }
    }
}
