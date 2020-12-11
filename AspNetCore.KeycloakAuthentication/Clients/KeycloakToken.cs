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
    }
}
