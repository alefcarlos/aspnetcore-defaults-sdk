using Ardalis.Result;
using Core;
using Mediator;

namespace UseCases.Create;

public class CreateTodoHandler : ICommandHandler<CreateTodoCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateTodoHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<Result> Handle(CreateTodoCommand command, CancellationToken cancellationToken)
    {
        var entity = new TodoEntity { Name = command.Name };

        _dbContext.Todos.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
