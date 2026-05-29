using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace AlefCarlos.AspNetCoreDefaults.WebApi.Authentication;

public sealed class AuthenticationDefaultsBuilder
{
    public IServiceCollection Services { get; }
    public IHostEnvironment Environment { get; }

    public AuthenticationBuilder Schemes { get; }

    public AuthenticationDefaultsBuilder(IServiceCollection services, IHostEnvironment environment)
    {
        Services = services;
        Environment = environment;

        Schemes = services.AddAuthentication();

        Services.ConfigureOptions<ConfigureAuthorizationDefaultsOption>();
        services.ConfigureOptions<ConfigureAuthorizationOptions>();
        Services.AddSingleton<IAuthorizationPolicyProvider, MultiSchemeAuthorizationPolicyProvider>();
    }

    public AuthenticationDefaultsBuilder AddJwtBearerDefaults()
    {
        //Quandor for development, habilitar uso do dotnet user jwts --scheme dotnet-user-jwts
        if (Environment.IsDevelopment())
        {
            Schemes.AddJwtBearer(AuthenticationDefaults.DotnetUserJwtScheme);
            SetDefaultScheme(AuthenticationDefaults.DotnetUserJwtScheme);
        }

        Services.AddJwtBearerOpenApiTransformers();
        Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptionsDefaults>());

        Services.Configure<HttpLoggingOptions>(options =>
        {
            options.ResponseHeaders.Add("WWW-Authenticate");
        });
        
        return this;
    }

    public AuthenticationDefaultsBuilder SetDefaultScheme(string scheme)
    {
        Services.Configure<AuthenticationOptions>(options =>
        {
            options.DefaultScheme = scheme;
        });

        return this;
    }
}
