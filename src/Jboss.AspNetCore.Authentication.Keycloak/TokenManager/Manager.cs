using Jboss.AspNetCore.Authentication.Keycloak.Clients;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Jboss.AspNetCore.Authentication.Keycloak.TokenManager
{
    [Obsolete("It will be incapsulated in the next release. Don't use this reference.")]
    public interface IManager
    {
        Task<HttpRequestMessage> AuthenticateAsync(HttpRequestMessage request, CancellationToken cancellationToken = default);
    }


    [Obsolete("It will be incapsulated in the next release. Don't use this reference.")]
    public class Manager : IManager
    {
        private readonly IKeycloakClient _keycloakClient;
        private readonly ClientInstallation _options;
        private readonly SemaphoreSlim _semaphore;
        private readonly ILogger _logger;
        private KeycloakToken _token;

        public Manager(IKeycloakClient keycloakClient, ClientInstallation options, ILogger<Manager> logger)
        {
            _semaphore = new SemaphoreSlim(1, 1);
            _keycloakClient = keycloakClient;
            _options = options;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        public async Task<HttpRequestMessage> AuthenticateAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Starting authenticating the request...");

            // prevent unnecessary locks
            if (_token == null || _token.Expired)
            {
                try
                {
                    _logger.LogTrace("Entering in lock section...");
                    await _semaphore.WaitAsync(cancellationToken);
                    _logger.LogTrace("Entered in lock section.");

                    // Ok. Needs a token but we have no one. Obtaining it...
                    if (_token == null)
                    {
                        _logger.LogDebug("No token found. Obtaining it with credentials...");
                        _token = await _keycloakClient.GetClientTokenAsync(
                            _options.Resource,
                            _options.Credentials.Secret,
                            cancellationToken);
                        _logger.LogDebug("Token was been obtained.");
                    }
                    // Access token was died and refresh token is not. So we need one more access token
                    else if (_token.Expired && !_token.RefreshExpired)
                    {
                        _logger.LogDebug("Acces token was died and refresh token is not. Obtaining new access token with refresh...");
                        _token = await _keycloakClient.GetClientTokenAsync(
                            _options.Resource,
                            _options.Credentials.Secret,
                            _token.RefreshToken,
                            cancellationToken);
                        _logger.LogDebug("New acces token was been obtained.");
                    }
                    // Both tokens died. Needs try again from the start
                    else if (_token.Expired && _token.RefreshExpired)
                    {
                        _logger.LogDebug("Access token and refresh token died. Obtaining it with credentials...");
                        _token = await _keycloakClient.GetClientTokenAsync(
                            _options.Resource,
                            _options.Credentials.Secret,
                            cancellationToken);
                        _logger.LogDebug("Token was been obtained.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in autoauth mechanism.");
                    _logger.LogDebug("Cleunup the token");
                    _token = null;
                    throw;
                }
                finally
                {
                    _logger.LogTrace("Releasing the lock...");
                    _semaphore.Release();
                    _logger.LogTrace("Lock was been released.");
                }
            }

            _logger.LogDebug("Authenticating the request...");
            request.Headers.Authorization = new AuthenticationHeaderValue(_token.TokenType, _token.AccessToken);
            _logger.LogInformation("Request was been authenticated.");

            return request;
        }
    }
}
