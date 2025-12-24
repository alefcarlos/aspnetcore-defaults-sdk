using Ardalis.Result;
using Core;
using Mediator;

namespace UseCases.GetAll;

public record GetAllTodosQuery() : IQuery<Result<List<TodoEntity>>>;
