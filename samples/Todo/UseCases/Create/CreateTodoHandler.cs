using Ardalis.Result;
using Mediator;

namespace UseCases.Create;

public class CreateTodoHandler : ICommandHandler<CreateTodoCommand, Result>
{
    public ValueTask<Result> Handle(CreateTodoCommand command, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(Result.Success());
    }
}
