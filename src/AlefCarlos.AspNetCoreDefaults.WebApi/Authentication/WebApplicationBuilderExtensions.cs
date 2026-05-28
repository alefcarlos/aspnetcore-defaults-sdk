using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder;

public static class WebApplicationBuilderExtensions
{
    extension(WebApplicationBuilder builder)
    {
        public AuthenticationDefaultsBuilder Authentication => new(builder.Configuration, builder.Services, builder.Environment);
    }
}

public sealed class AuthenticationDefaultsBuilder
{
    public IConfiguration Configuration { get; }
    public IServiceCollection Services { get; }
    public IHostEnvironment Environment { get; }

    public AuthenticationBuilder Schemes { get; }

    public AuthenticationDefaultsBuilder(IConfiguration configuration, IServiceCollection services, IHostEnvironment environment)
    {
        Configuration = configuration;
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
