using AlefCarlos.AspNetCoreDefaults.WebApi.Authentication;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServicesCollectionExtensionsExtensions
{
    extension(IServiceCollection services)
    {
        public AuthenticationDefaultsBuilder AddAuthenticationDefaults(IHostEnvironment environment) => new(services, environment);
    }
}
