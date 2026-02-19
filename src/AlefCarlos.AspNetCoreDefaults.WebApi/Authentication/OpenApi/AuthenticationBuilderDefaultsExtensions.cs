using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.AmbientMetadata;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class AuthenticationBuilderDefaultsExtensions
{
    public static AuthenticationBuilder AddJwtBearerDefaults(this AuthenticationBuilder builder, string scheme = JwtBearerDefaults.AuthenticationScheme)
    {
        builder.Services.AddJwtBearerOpenApiTransformers();

        builder.AddJwtBearer(scheme);
        builder.Services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>>(sp =>
        {
            var applicationMetadata = sp.GetRequiredService<IOptions<ApplicationMetadata>>();

            return new ConfigureJwtBearerOptionsDefaults(applicationMetadata.Value, scheme);
        });

        return builder;
    }
}
