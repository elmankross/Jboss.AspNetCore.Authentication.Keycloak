using AspNetCore.KeycloakAuthentication.Clients;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore.KeycloakAuthentication.Handlers
{
    public class HttpKeycloakAutoSigningHandler : DelegatingHandler
    {
        private readonly IKeycloakClient _keycloakClient;
        private readonly KeycloakClientInstallation _options;
        private KeycloakToken _token;

        public HttpKeycloakAutoSigningHandler(IKeycloakClient keycloakClient, KeycloakClientInstallation options)
        {
            _keycloakClient = keycloakClient;
            _options = options;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return response;
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _token ??= await _keycloakClient.GetClientTokenAsync(_options.Resource, _options.Credentials.Secret);

                var jwt = new JwtSecurityToken(_token.AccessToken);
                var unixTime = DateTimeOffset.Now.ToUnixTimeSeconds();
                if (jwt.Payload.Exp.HasValue && unixTime > jwt.Payload.Exp)
                {
                    _token = await _keycloakClient.GetClientTokenAsync(_options.Resource, _options.Credentials.Secret, _token.RefreshToken);
                }

                request.Headers.Authorization = new AuthenticationHeaderValue(_token.TokenType, _token.AccessToken);
                response = await base.SendAsync(request, cancellationToken);
            }

            return response;
        }
    }
}
