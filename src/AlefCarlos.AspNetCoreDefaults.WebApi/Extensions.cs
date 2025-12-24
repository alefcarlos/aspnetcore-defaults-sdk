using AspNetCoreDefaults.WebApi.OpenApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.AmbientMetadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

namespace Microsoft.Extensions.Hosting;

public static class Extensions
{
    public static WebApplicationBuilder AddWebApiDefaults(this WebApplicationBuilder builder)
    {
        builder.AddDefaults();

        builder.Services.AddProblemDetails();

        builder.Services.AddOptions<OpenApiInfo>()
            .Configure<IOptions<ApplicationMetadata>>((options, appInfo) =>
            {
                options.Title = appInfo.Value.ApplicationName;
                options.Version = appInfo.Value.BuildVersion;
            });

        builder.Services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<OpenApiInfoTransformer>();
        });

        return builder;
    }

    public static void UseProblemDetailsWithDefaults(this WebApplication app)
    {
        app.UseExceptionHandler();
        app.UseStatusCodePages();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
    }

    public static WebApplication MapDefaultWebApiEndpoints(this WebApplication app)
    {
        app.MapDefaultEndpoints();
        app.MapOpenApi();
        app.MapScalarApiReference("/docs");
        
        return app;
    }
}
