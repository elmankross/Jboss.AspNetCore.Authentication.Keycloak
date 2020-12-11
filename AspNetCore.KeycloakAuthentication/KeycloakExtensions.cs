using AspNetCore.KeycloakAuthentication.Clients;
using AspNetCore.KeycloakAuthentication.Handlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net.Http;

namespace AspNetCore.KeycloakAuthentication
{
    public static class KeycloakExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static IWebHostBuilder UseKeycloak(this IWebHostBuilder host)
        {
            return host.ConfigureAppConfiguration((_, builder) =>
            {
                builder.AddJsonFile(KeycloakClientInstallation.FILE, optional: true);
            });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddKeycloakAuthentication(this IServiceCollection services,
            IConfiguration configuration,
            Action<KeycloakClientInstallation> options = null)
        {
            var installation = new KeycloakClientInstallation();
            configuration.Bind(installation);
            options?.Invoke(installation);
            services.AddSingleton(installation);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(x =>
                    {
                        x.Authority = installation.Issuer.ToString();
                        x.SaveToken = true;

                        // TODO: By env
                        x.RequireHttpsMetadata = false;
                        x.IncludeErrorDetails = false;
                        x.RefreshOnIssuerKeyNotFound = false;

                        x.TokenValidationParameters = new TokenValidationParameters
                        {
                            RequireSignedTokens = false,
                            ValidateAudience = false,
                            ValidateLifetime = true,
                            ValidateIssuer = true,
                            ValidIssuer = installation.Issuer.ToString(),
                            ValidateIssuerSigningKey = true,
                            TryAllIssuerSigningKeys = true
                        };

                        x.Validate();
                    });

            return services;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IHttpClientBuilder AddKeycloakHttpClient<TClient, TImplementation>(this IServiceCollection services,
            Action<HttpClient> config = null)
            where TClient : class
            where TImplementation : class, TClient
        {
            services.AddSingleton<IKeycloakClient, KeycloakClient>();
            services.AddTransient<HttpKeycloakAutoSigningHandler>();
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
        public static IHttpClientBuilder AddKeycloakHttpClient<TImplementation>(this IServiceCollection services,
            Action<HttpClient> config = null)
            where TImplementation : class
        {
            services.AddSingleton<IKeycloakClient, KeycloakClient>();
            return services.AddHttpClient<TImplementation>(x => config?.Invoke(x))
                           .AddHttpMessageHandler<HttpKeycloakAutoSigningHandler>();
        }
    }
}
