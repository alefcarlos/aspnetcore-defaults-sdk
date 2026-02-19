using AlefCarlos.AspNetCoreDefaults.WebApi.Authentication.OpenApi;
using Microsoft.AspNetCore.OpenApi;

namespace Microsoft.Extensions.DependencyInjection;

public static class JwtBearerOpenApiExtensions
{
    public static IServiceCollection AddJwtBearerOpenApiTransformers(this IServiceCollection services)
    {
        services.Configure<OpenApiOptions>("v1", options =>
        {
            options.AddOperationTransformer<BearerAuthOperationTransformer>();
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
        });

        return services;
    }
}
