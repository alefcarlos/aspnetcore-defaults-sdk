using Ardalis.Result.AspNetCore;
using Mediator;
using UseCases.Get;
using UseCases.GetAll;

namespace TodoApi.Endpoints.GetTodo
{
    public static class Extensions
    {
        public static IEndpointRouteBuilder MapGetTodo(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("{id:guid}", async (IMediator mediator, Guid id) =>
            {
                var result = await mediator.Send(new GetTodoQuery(new(id)));

                return result.ToMinimalApiResult();
            });

            endpoints.MapGet("", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetAllTodosQuery());

                return result.ToMinimalApiResult();
            });

            return endpoints;
        }
    }
}
