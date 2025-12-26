using Microsoft.AspNetCore.OpenApi;
using TodoApi.OpenApi;

namespace Microsoft.Extensions.DependencyInjection;

public static class Extensions
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