using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;
using Neovortex.Mediator.OpenTelemetry;
using TodoApi.Endpoints;
using UseCases;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks().AddCheck<StartupDelayHealthCheck>("degraded", tags: ["live"]);

builder.AddWebApiDefaults();
builder.AddInfra();

builder.Services.AddAuthentication()
    .AddJwtBearer()
    ;

builder.Services.AddJwtBearerOpenApiTransformers();

builder.Services.AddAuthorizationBuilder().AddPolicy("Alef", p=> p.RequireRole("role")) ;

builder.Services.AddValidation();

builder.Services.AddMediator(options =>
{
    options.ServiceLifetime = ServiceLifetime.Scoped;

    // Supply any TYPE from each assembly you want scanned (the generator finds the assembly from the type)
    options.Assemblies =
    [
        typeof(IUseCasesMarker),
    ];
});

builder.Services.AddMediatorOpenTelemetry();
builder.Services.AddOpenTelemetry().WithTracing(t=> t.AddMediatorInstrumentation());

builder.Services.AddValidatorsFromAssemblies([typeof(IUseCasesMarker).Assembly]);

var app = builder.Build();

app.UseHttpLogging();

app.UseProblemDetailsWithDefaults();

app.MapDefaultWebApiEndpoints();

app.UseAuthentication();
app.UseAuthorization();

app.MapTodoEndpoints();
app.MapGet("ping", () => "pong");

app.Run();

public class Teste : DefaultAuthorizationPolicyProvider
{
    private readonly AuthorizationOptions _options;

    public Teste(IOptions<AuthorizationOptions> options) : base(options)
    {
        _options = options.Value;
    }

    public override Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        return base.GetPolicyAsync(policyName);
    }
}
