using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
var isWebApiSut = builder.Configuration.GetValue("WEBAPI_SUT", false);

if (!isWebApiSut)
{
    builder.AddDefaults();
}
else
{
    builder.AddWebApiDefaults();
}

var app = builder.Build();


if (!isWebApiSut)
{
    app.MapDefaultEndpoints();
}
else
{
    app.UseProblemDetailsWithDefaults();

    app.MapDefaultWebApiEndpoints();

    app.MapGet("/test/ok", () => Results.Ok(new { message = "ok" }));

    app.MapGet("/test/error", () =>
    {
        throw new InvalidOperationException("test error");
    });
}

app.Run();

