using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Jboss.AspNetCore.Authentication.Keycloak.Handlers
{
    [Obsolete("It will be incapsulated in the next release. Don't use this reference.")]
    public class HttpKeycloakAutoSigningHandler : DelegatingHandler
    {
        private readonly TokenManager.IManager _manager;

        public HttpKeycloakAutoSigningHandler(TokenManager.IManager manager)
        {
            _manager = manager;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request = await _manager.AuthenticateAsync(request);
            var response = await base.SendAsync(request, cancellationToken);
            return response;
        }
    }
}
