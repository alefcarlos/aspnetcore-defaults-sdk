using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;

namespace AspNetCoreDefaults.WebApi.OpenApi;

internal sealed class OpenApiInfoTransformer(IOptions<OpenApiInfo> options) : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        document.Info = options.Value;
    }
}