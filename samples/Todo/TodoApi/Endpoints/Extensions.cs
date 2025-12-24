using TodoApi.Endpoints.CreateTodo;
using TodoApi.Endpoints.GetTodo;

namespace TodoApi.Endpoints
{
    public static class Extensions
    {
        public static IEndpointRouteBuilder MapTodoEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group =  endpoints.MapGroup("v1/todos")
                .WithTags("Todos")
                ;

            group.MapCreateTodo();
            group.MapGetTodo();

            return endpoints;
        }
    }
}
