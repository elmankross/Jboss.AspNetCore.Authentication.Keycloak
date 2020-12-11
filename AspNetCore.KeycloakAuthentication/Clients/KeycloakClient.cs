using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AspNetCore.KeycloakAuthentication.Clients
{
    /// <summary>
    /// 
    /// </summary>
    public interface IKeycloakClient
    {
        ValueTask<KeycloakToken> GetClientTokenAsync(string clientId, string secretKey);
        ValueTask<KeycloakToken> GetClientTokenAsync(string clientId, string secretKey, string refreshToken);
        ValueTask<KeycloakToken> GetUserTokenAsync(string userName, string password, string clientId, string secretKey);
    }


    /// <summary>
    /// 
    /// </summary>
    public class KeycloakClient : IKeycloakClient
    {
        private readonly HttpClient _httpClient;
        private readonly KeycloakClientInstallation _installation;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="installation"></param>
        public KeycloakClient(HttpClient httpClient, KeycloakClientInstallation installation)
        {
            _httpClient = httpClient;
            _installation = installation;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        public ValueTask<KeycloakToken> GetClientTokenAsync(string clientId, string secretKey)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _installation.TokenEndpoint)
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["client_id"] = clientId,
                    ["client_secret"] = secretKey,
                    ["grant_type"] = "client_credentials"
                })
            };

            return ExecuteAsync<KeycloakToken>(request);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="secretKey"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public ValueTask<KeycloakToken> GetClientTokenAsync(string clientId, string secretKey, string refreshToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _installation.TokenEndpoint)
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["client_id"] = clientId,
                    ["client_secret"] = secretKey,
                    ["refresh_token"] = refreshToken,
                    ["grant_type"] = "refresh_token"
                })
            };

            return ExecuteAsync<KeycloakToken>(request);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="clientId"></param>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        public ValueTask<KeycloakToken> GetUserTokenAsync(string userName, string password, string clientId, string secretKey)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _installation.TokenEndpoint)
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["client_id"] = clientId,
                    ["client_secret"] = secretKey,
                    ["username"] = userName,
                    ["password"] = password,
                    ["grant_type"] = "client_credentials"
                })
            };

            return ExecuteAsync<KeycloakToken>(request);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async ValueTask<TResponse> ExecuteAsync<TResponse>(HttpRequestMessage request)
        {
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            var payload = await JsonSerializer.DeserializeAsync<TResponse>(stream);
            return payload;
        }
    }
}
