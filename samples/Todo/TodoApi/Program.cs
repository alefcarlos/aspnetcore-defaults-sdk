using FluentValidation;
using TodoApi.Endpoints;
using UseCases;

var builder = WebApplication.CreateBuilder(args);

builder.AddWebApiDefaults();
builder.AddInfra();

builder.Services.AddAuthentication()
    .AddJwtBearer()
    ;

builder.Services.AddJwtBearerOpenApiTransformers();

builder.Services.AddAuthorization();

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
