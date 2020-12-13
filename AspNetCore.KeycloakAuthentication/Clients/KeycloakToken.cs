using System;
using System.Text.Json.Serialization;

namespace AspNetCore.KeycloakAuthentication.Clients
{
    public class KeycloakToken
    {
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresInSeconds { get; set; }

        [JsonPropertyName("refresh_expires_in")]
        public int RefreshExpiresInSeconds { get; set; }

        /// <summary>
        /// Revocation server time. By default it's server's start time  
        /// </summary>
        [JsonPropertyName("not-before-policy")]
        public long RevocationUnixTime { get; set; }

        [JsonPropertyName("session_state")]
        public string SessionState { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }


        /// <summary>
        /// Time when token was been obtained
        /// </summary>
        [JsonIgnore]
        public DateTime TokenTime { get; }

        [JsonIgnore]
        public DateTime RevocationTime => DateTimeOffset.FromUnixTimeSeconds(RevocationUnixTime).DateTime;

        [JsonIgnore]
        public DateTime AccessTokenExperationTime => TokenTime.AddSeconds(ExpiresInSeconds);

        [JsonIgnore]
        public DateTime RefreshTokenExperationTime => TokenTime.AddSeconds(RefreshExpiresInSeconds);

        [JsonConstructor]
        public KeycloakToken()
        {
            TokenTime = DateTime.Now;
        }
    }
}
