using AlefCarlos.AspNetCoreDefaults.WebApi.Authentication;

namespace Microsoft.AspNetCore.Builder;

public static class WebApplicationBuilderExtensions
{
    extension(WebApplicationBuilder builder)
    {
        public AuthenticationDefaultsBuilder Authentication => new(builder.Services, builder.Environment);
    }
}