using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Jboss.AspNetCore.Authentication.Keycloak.Clients
{
    /// <summary>
    /// 
    /// </summary>
    [Obsolete("It will be incapsulated in the next release. Don't use this reference.")]
    public interface IKeycloakClient
    {
        Task<KeycloakToken> GetClientTokenAsync(string clientId, string secretKey, 
            CancellationToken cancellationToken = default);
        Task<KeycloakToken> GetClientTokenAsync(string clientId, string secretKey, string refreshToken,
            CancellationToken cancellationToken = default);
        Task<KeycloakToken> GetUserTokenAsync(string userName, string password, string clientId, string secretKey,
            CancellationToken cancellationToken = default);
    }


    /// <summary>
    /// 
    /// </summary>
    [Obsolete("It will be incapsulated in the next release. Don't use this reference.")]
    public class KeycloakClient : IKeycloakClient
    {
        private readonly HttpClient _httpClient;
        private readonly ClientInstallation _installation;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="installation"></param>
        public KeycloakClient(HttpClient httpClient, ClientInstallation installation)
        {
            _httpClient = httpClient;
            _installation = installation;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="secretKey"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<KeycloakToken> GetClientTokenAsync(string clientId, string secretKey,
            CancellationToken cancellationToken = default)
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

            return ExecuteAsync<KeycloakToken>(request, cancellationToken);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="secretKey"></param>
        /// <param name="refreshToken"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<KeycloakToken> GetClientTokenAsync(string clientId, string secretKey, string refreshToken,
            CancellationToken cancellationToken = default)
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

            return ExecuteAsync<KeycloakToken>(request, cancellationToken);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="clientId"></param>
        /// <param name="secretKey"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<KeycloakToken> GetUserTokenAsync(string userName, string password, string clientId, string secretKey,
            CancellationToken cancellationToken = default)
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

            return ExecuteAsync<KeycloakToken>(request, cancellationToken);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<TResponse> ExecuteAsync<TResponse>(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await _httpClient.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new Exception(content);
            }
            var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var payload = await JsonSerializer.DeserializeAsync<TResponse>(stream, cancellationToken: cancellationToken);
            return payload;
        }
    }
}
