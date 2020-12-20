# Description

Keycloak authentication extension (auth adapter) with predefined configuration and authorization policies requirements.
It's simplify integration with auth service, all you needs it's just drop the installation keycloak client's file
(`keycloak.json`) in root application folder.

## Setup

### Authentication

1. Allow to application reads client installation file from the root directory

```csharp
public class Program
{
    public static void Main(string[] args)
    {
        CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
                // New line 
                .UseKeycloak()
                .UseStartup<Startup>();
}
```

2. Add Keycloak authentication functionality and authorization policies

```csharp
public void ConfigureServices(IServiceCollection services)
{  
    // New line 
    services.AddKeycloakAuthentication(Configuration);

    services.AddAuthorization(config => /* ... */);
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseAuthentication();
    app.UseAuthorization();
}
```

3. Enjoy

### Autosigning HttpClient

1. Do first two steps from `Authentication` section
2. Configure http client

```csharp
public void ConfigureServices(IServiceCollection services)
{

    services.AddKeycloakHttpClient<IMicroserviceTwo, MicroserviceTwo>(c => { /* ... */ });
    services.AddKeycloakHttpClient<IMicroserviceThree, MicroserviceThree>(c => { /* ... */ });
}
```

3. Done. Just invoke `IMicroserviceTwo` or `IMicroserviceThree` whenever and don't think about auth tokens.

# Features
1. Authentication with autosetup through `keycloak.json` file
2. HttpClient with auto obtaining and refreshing access_token
3. Predefined authorization policy requirements  
   3.1. `ResourceAccess` - allows to check roles from different resources

# TODO
[ ] More predefined policy requirements  
[ ] Importing Authorization configuration from Keycloak ([service-id]-authz-config.json)
