using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace AlefCarlos.AspNetCoreDefaults.WebApi.Authentication;

internal class ConfigureAuthorizationDefaultsOption : IPostConfigureOptions<AuthorizationDefaultsOption>
{
    private readonly AuthenticationOptions _options;

    public ConfigureAuthorizationDefaultsOption(IOptions<AuthenticationOptions> options)
    {
        _options = options.Value;
    }

    public void PostConfigure(string? name, AuthorizationDefaultsOption options)
    {
        options.EnableMultiSchemeDefaultPolicy = _options.Schemes.Any();
    }
}
