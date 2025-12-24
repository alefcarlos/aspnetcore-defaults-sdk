using Ardalis.Result;
using Mediator;

namespace UseCases.Create;

public record CreateTodoCommand(string Name) : ICommand<Result>;
