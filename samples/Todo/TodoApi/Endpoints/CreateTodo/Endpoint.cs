using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UseCases.Create;

namespace TodoApi.Endpoints.CreateTodo;

public static class Extensions
{
    public static IEndpointRouteBuilder MapCreateTodo(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("", CreateAsync);

        return endpoints;
    }

    private static async ValueTask<Results<Created, ValidationProblem, ProblemHttpResult>> CreateAsync(IMediator mediator, [FromBody] CreateTodoRequest request)
    {
        var result = await mediator.Send(new CreateTodoCommand(request.Name));

        return result.ToCreatedResult();
    }
}
