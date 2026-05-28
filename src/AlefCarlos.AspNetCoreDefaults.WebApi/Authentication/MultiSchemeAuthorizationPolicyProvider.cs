using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder;

internal sealed class MultiSchemeAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    private readonly AuthorizationDefaultsOption _authorizationDefaultsOption;
    private readonly string[] _schemes;
    public MultiSchemeAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options, IOptions<AuthenticationOptions> authenticationOptions, IOptions<AuthorizationDefaultsOption> authorizationDefaultsOption) : base(options)
    {
        _authorizationDefaultsOption = authorizationDefaultsOption.Value;
        _schemes = [.. authenticationOptions.Value.Schemes.Select(s => s.Name)];
    }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policy = await base.GetPolicyAsync(policyName);

        if (!_authorizationDefaultsOption.EnableMultiSchemeDefaultPolicy)
        {
            return policy;
        }

        if (policy is null)
            return policy;

        if (policy.AuthenticationSchemes.Any())
        {
            //Verificar se a policy tem scheme do dotnet-user-jwt, se não tiver e o esquema estiver registrado, adiciona.
            if (!policy.AuthenticationSchemes.Contains(AuthenticationDefaults.DotnetUserJwtScheme) && _schemes.Contains(AuthenticationDefaults.DotnetUserJwtScheme))
            {
                return new AuthorizationPolicyBuilder(policy)
                    .AddAuthenticationSchemes(AuthenticationDefaults.DotnetUserJwtScheme)
                    .Build();
            }

            return policy;
        }

        //Caso não tenha nenhum esquema registrado, retornar a policy original, caso contrário, adicionar os esquemas registrados.
        if (_schemes.Length > 0)
        {
            return new AuthorizationPolicyBuilder(policy)
                .AddAuthenticationSchemes(_schemes)
                .Build();
        }

        return policy;
    }
}