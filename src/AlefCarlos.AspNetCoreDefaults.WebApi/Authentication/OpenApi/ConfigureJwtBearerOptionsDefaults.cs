using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.AmbientMetadata;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

internal class ConfigureJwtBearerOptionsDefaults : IPostConfigureOptions<JwtBearerOptions>
{
    private readonly ApplicationMetadata _applicationMetadata;

    public ConfigureJwtBearerOptionsDefaults(IOptions<ApplicationMetadata> applicationMetadata)
    {
        _applicationMetadata = applicationMetadata.Value;
    }

    public void PostConfigure(string? name, JwtBearerOptions options)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return;
        }

        options.TokenValidationParameters.ValidAudience = _applicationMetadata.ApplicationName;
    }
}