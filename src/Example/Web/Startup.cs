using Jboss.AspNetCore.Authentication.Keycloak;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Web
{
    using Services;
    using Services.Clients;

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
            var uri1 = Configuration.GetValue<Uri>("Services:First");
            var uri2 = Configuration.GetValue<Uri>("Services:Second");

            services.AddTransient<ITestService, TestService>();

            services.AddHttpClient<IMicroserviceFirst, MicroserviceFirst>(c => c.BaseAddress = uri1)
                    .AddKeycloakSupport();
            services.AddHttpClient<IMicroserviceSecond, MicroserviceSecond>(c => c.BaseAddress = uri2)
                    .AddKeycloakSupport();

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
