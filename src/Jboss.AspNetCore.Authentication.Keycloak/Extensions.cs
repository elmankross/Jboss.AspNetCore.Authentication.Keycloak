using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net.Http;

namespace Jboss.AspNetCore.Authentication.Keycloak
{
    using Clients;
    using Handlers;
    using Providers.KeycloakConfiguration;
    using PolicyRequirements.ResourceAccess;

    public static class Extensions
    {
        private static ClientInstallation _installation = new ClientInstallation();
        private static bool _installationRegistered = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static IWebHostBuilder UseKeycloak(this IWebHostBuilder host)
        {
            _installationRegistered = true;
            return host.ConfigureAppConfiguration((_, builder) =>
            {
                var source = new KeycloakConfigurationSource
                {
                    Path = ClientInstallation.FILE,
                    Optional = false
                };
                builder.Sources.Insert(0, source);
            }).ConfigureServices((builder, services) =>
            {
                builder.Configuration.Bind(KeycloakConfigurationProvider.CONFIGURATION_PREFIX, _installation);
                services.AddSingleton(_installation);
            });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddKeycloakAuthentication(this IServiceCollection services)
        {
            EnsureInstallationRegistered();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(x =>
                    {
                        x.Authority = _installation.Issuer.ToString();
                        x.SaveToken = true;

                        // TODO: By env
                        x.RequireHttpsMetadata = false;
                        x.IncludeErrorDetails = false;
                        x.RefreshOnIssuerKeyNotFound = false;

                        x.TokenValidationParameters = new TokenValidationParameters
                        {
                            RequireSignedTokens = false,
                            ValidateAudience = _installation.VerifyTokenAudience,
                            ValidAudience = _installation.Resource,
                            ValidateLifetime = true,
                            ValidateIssuer = true,
                            ValidIssuer = _installation.Issuer.ToString(),
                            ValidateIssuerSigningKey = true
                        };

                        x.Validate();
                    });

            services.AddSingleton<IAuthorizationHandler, ResourceAccessRequirement>();

            return services;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHttpClientBuilder AddKeycloakSupport(this IHttpClientBuilder builder)
        {
            EnsureInstallationRegistered();

            builder.Services.AddSingleton<IKeycloakClient, KeycloakClient>();
            builder.Services.AddSingleton<TokenManager.IManager, TokenManager.Manager>();
            builder.Services.AddScoped<HttpKeycloakAutoSigningHandler>();

            return builder.AddHttpMessageHandler<HttpKeycloakAutoSigningHandler>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        [Obsolete("Use .AddKeycloakSupport() on .AddHttpClient<,> instead.")]
        public static IHttpClientBuilder AddKeycloakHttpClient<TClient, TImplementation>(this IServiceCollection services,
            Action<HttpClient> config = null)
            where TClient : class
            where TImplementation : class, TClient
        {
            EnsureInstallationRegistered();

            services.AddSingleton<IKeycloakClient, KeycloakClient>();
            services.AddSingleton<TokenManager.IManager, TokenManager.Manager>();

            services.AddScoped<HttpKeycloakAutoSigningHandler>();
            return services.AddHttpClient<TClient, TImplementation>(x => config?.Invoke(x))
                           .AddHttpMessageHandler<HttpKeycloakAutoSigningHandler>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        [Obsolete("Use .AddKeycloakSupport() on .AddHttpClient<,> instead.")]
        public static IHttpClientBuilder AddKeycloakHttpClient<TImplementation>(this IServiceCollection services,
            Action<HttpClient> config = null)
            where TImplementation : class
        {
            EnsureInstallationRegistered();

            services.AddSingleton<IKeycloakClient, KeycloakClient>();
            return services.AddHttpClient<TImplementation>(x => config?.Invoke(x))
                           .AddHttpMessageHandler<HttpKeycloakAutoSigningHandler>();
        }


        /// <summary>
        /// 
        /// </summary>
        private static void EnsureInstallationRegistered()
        {
            if (!_installationRegistered)
            {
                throw new Exception("It needs to use .UseKeycloak() on IWebHostBuilder before.");
            }
        }
    }
}
