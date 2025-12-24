using Ardalis.Result.AspNetCore;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using UseCases.Create;

namespace TodoApi.Endpoints.CreateTodo
{
    public static class Extensions
    {
        public static IEndpointRouteBuilder MapCreateTodo(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("", async (IMediator mediator, [FromBody] CreateTodoRequest request) =>
            {
                var result = await mediator.Send(new CreateTodoCommand(request.Name));

                return result.ToMinimalApiResult();
            });


            return endpoints;
        }
    }
}
