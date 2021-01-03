using System;
using System.Text.Json.Serialization;

namespace Jboss.AspNetCore.Authentication.Keycloak.Clients
{
    [Obsolete("It will be incapsulated in the next release. Don't use this reference.")]
    public class KeycloakToken
    {
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("session_state")]
        public string SessionState { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresInSeconds { private get; set; }

        [JsonPropertyName("refresh_expires_in")]
        public int RefreshExpiresInSeconds { private get; set; }

        /// <summary>
        /// Revocation server time. By default it's server's start time  
        /// </summary>
        [JsonPropertyName("not-before-policy")]
        public long RevocationUnixTime { private get; set; }

        [JsonIgnore]
        public bool Expired => _tokenTime.AddSeconds(ExpiresInSeconds) <= DateTime.Now;

        [JsonIgnore]
        public bool RefreshExpired => _tokenTime.AddSeconds(RefreshExpiresInSeconds) <= DateTime.Now;

        [JsonIgnore]
        private DateTime RevocationTime => DateTimeOffset.FromUnixTimeSeconds(RevocationUnixTime).DateTime;

        /// <summary>
        /// Time when token was been obtained
        /// </summary>
        [JsonIgnore]
        private DateTime _tokenTime;

        [JsonConstructor]
        public KeycloakToken()
        {
            _tokenTime = DateTime.Now;
        }
    }
}
