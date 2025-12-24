using FluentValidation;
using TodoApi.Endpoints;
using UseCases;
using UseCases.Behaviours;

var builder = WebApplication.CreateBuilder(args);

builder.AddWebApiDefaults();

builder.Services.AddMediator(options =>
{
    options.ServiceLifetime = ServiceLifetime.Scoped;

    // Supply any TYPE from each assembly you want scanned (the generator finds the assembly from the type)
    options.Assemblies =
    [
        typeof(IUseCasesMarker),
    ];

    options.PipelineBehaviors =
    [
        typeof(MessageValidatorBehaviour<,>),
    ];
});

builder.Services.AddValidatorsFromAssemblies([typeof(IUseCasesMarker).Assembly]);

var app = builder.Build();

app.UseHttpLogging();

app.UseProblemDetailsWithDefaults();

app.MapDefaultWebApiEndpoints();

app.MapTodoEndpoints();

app.Run();
