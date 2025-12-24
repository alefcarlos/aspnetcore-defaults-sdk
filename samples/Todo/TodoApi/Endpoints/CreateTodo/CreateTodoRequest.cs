using System.ComponentModel.DataAnnotations;

namespace TodoApi.Endpoints.CreateTodo;

public record CreateTodoRequest([Required]string Name);
