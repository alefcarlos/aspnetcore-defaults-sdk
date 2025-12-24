using Ardalis.Result;
using Core;
using Mediator;

namespace UseCases.Get;

public record GetTodoQuery(TodoId Id) : IQuery<Result<TodoEntity>>;
