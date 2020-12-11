using AspNetCore.Authentication.WApp.Services;
using AspNetCore.Authentication.WApp.Services.Clients;
using AspNetCore.KeycloakAuthentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace AspNetCore.Authentication.WApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddKeycloakAuthentication(Configuration);

            var uri2 = Configuration.GetValue<Uri>("UrlServices:microservices-two");
            var uri3 = Configuration.GetValue<Uri>("UrlServices:microservices-three");

            services.AddTransient<ITestService, TestService>();

            services.AddKeycloakHttpClient<IMicroserviceTwo, MicroserviceTwo>(c => c.BaseAddress = uri2);
            services.AddKeycloakHttpClient<IMicroserviceThree, MicroserviceThree>(c => c.BaseAddress = uri3);

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication(); //Habilitar la autenticación de Keycloak

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
