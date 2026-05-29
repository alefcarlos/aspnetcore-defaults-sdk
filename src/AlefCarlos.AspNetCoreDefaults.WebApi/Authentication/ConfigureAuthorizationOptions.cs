using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace AlefCarlos.AspNetCoreDefaults.WebApi.Authentication;

internal class ConfigureAuthorizationOptions : IPostConfigureOptions<AuthorizationOptions>
{
    private readonly AuthenticationOptions _options;
    private readonly AuthorizationDefaultsOption _authorizationDefaultsOption;

    public ConfigureAuthorizationOptions(IOptions<AuthenticationOptions> options, IOptions<AuthorizationDefaultsOption> authorizationDefaultsOption)
    {
        _options = options.Value;
        _authorizationDefaultsOption = authorizationDefaultsOption.Value;
    }

    public void PostConfigure(string? name, AuthorizationOptions options)
    {
        if (_authorizationDefaultsOption.EnableMultiSchemeDefaultPolicy)
        {
            string[] schemes = [.. _options.Schemes.Select(s => s.Name)];

            options.DefaultPolicy = new AuthorizationPolicyBuilder(schemes)
                .RequireAuthenticatedUser()
                .Build();
        }
    }
}
