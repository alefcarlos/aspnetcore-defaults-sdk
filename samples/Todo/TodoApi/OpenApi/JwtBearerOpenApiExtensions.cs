using Microsoft.AspNetCore.OpenApi;
using TodoApi.OpenApi;

namespace Microsoft.Extensions.DependencyInjection;

internal static class JwtBearerOpenApiExtensions
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