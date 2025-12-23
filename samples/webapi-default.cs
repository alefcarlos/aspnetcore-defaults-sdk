#:sdk Microsoft.NET.Sdk.Web
#:property PublishAot=false
#:property IncludeSourceRevisionInInformationalVersion=false
#:project ../src/metapackages/AspNetCoreDefaults.WebApi.All

using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.AddWebApiDefaults();

builder.Services.Configure<OpenApiInfo>(opts => opts.Description = "Description from this api bla bla bla");

var app = builder.Build();

app.UseHttpLogging();

app.UseProblemDetailsWithDefaults();

app.MapGet("/", () => new HelloResponse { Message = "Hello, World!" })
    .WithName("HelloWorld");

app.MapDefaultWebApiEndpoints();

app.Run();

class HelloResponse
{
    public string Message { get; set; } = "Hello, World!";
}
