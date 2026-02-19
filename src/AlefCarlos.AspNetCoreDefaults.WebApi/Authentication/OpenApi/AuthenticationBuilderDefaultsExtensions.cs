using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class AuthenticationBuilderDefaultsExtensions
{
    public static AuthenticationBuilder AddJwtBearerDefaults(this AuthenticationBuilder builder, string scheme = JwtBearerDefaults.AuthenticationScheme)
    {
        builder.Services.AddJwtBearerOpenApiTransformers();

        builder.AddJwtBearer(scheme);
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptionsDefaults>());

        return builder;
    }
}
