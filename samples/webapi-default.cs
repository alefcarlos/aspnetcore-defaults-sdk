#:sdk Microsoft.NET.Sdk.Web
#:property PublishAot=false
#:property UserSecretsId=13e6dfb7-860b-4703-ab0f-ae7e75fcfbb5
#:property IncludeSourceRevisionInInformationalVersion=false
#:project ../src/metapackages/AlefCarlos.AspNetCore.WebApi

using System.Text.Json.Serialization;
using Microsoft.Extensions.AmbientMetadata;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

builder.AddWebApiDefaults();

builder.Authentication
    .AddJwtBearerDefaults();

builder.Services.AddAuthorization();

builder.Services.Configure<OpenApiInfo>(opts => opts.Description = "Description for this api bla bla bla");

var app = builder.Build();

app.UseHttpLogging();

app.UseProblemDetailsWithDefaults();

app.MapGet("/", () => new HelloResponse("Hello, World!"))
    .WithName("HelloWorld");

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/ping", (HttpContext context, ILogger<HelloResponse> l, IConfiguration c) =>
{
    static string GetAuthorizationScheme(HttpRequest request) =>
        request.Headers.Authorization.First()!.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0];

    static string GetAccessToken(HttpRequest request) =>
        request.Headers.Authorization.First()!.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1];

    var claims = context.User.Claims.Select(c => new KeyValuePair<string, string>(c.Type, c.Value));
    var scheme = GetAuthorizationScheme(context.Request);
    var accessToken = GetAccessToken(context.Request);

    return new WhoAmI(scheme, claims, accessToken);
}).RequireAuthorization();

app.MapDefaultWebApiEndpoints();

app.Run();

record HelloResponse(string Message);
record WhoAmI(string Scheme, IEnumerable<KeyValuePair<string, string>> Claims, string AccessToken);

[JsonSerializable(typeof(HelloResponse))]
[JsonSerializable(typeof(WhoAmI))]
[JsonSerializable(typeof(ApplicationMetadata))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}