using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.AmbientMetadata;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

internal class ConfigureJwtBearerOptionsDefaults : IPostConfigureOptions<JwtBearerOptions>
{
    private readonly string _scheme;
    private readonly ApplicationMetadata _applicationMetadata;

    public ConfigureJwtBearerOptionsDefaults(ApplicationMetadata applicationMetadata, string scheme)
    {
        _applicationMetadata = applicationMetadata;
        _scheme = scheme;
    }

    public void PostConfigure(string? name, JwtBearerOptions options)
    {
        if (_scheme != name)
        {
            return;
        }

        options.TokenValidationParameters.ValidAudience = _applicationMetadata.ApplicationName;
    }
}