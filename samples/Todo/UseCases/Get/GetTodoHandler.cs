using Ardalis.Result;
using Core;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace UseCases.Get;

public class GetTodoHandler : IQueryHandler<GetTodoQuery, Result<TodoEntity>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetTodoHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<Result<TodoEntity>> Handle(GetTodoQuery query, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Todos.FirstOrDefaultAsync(x=>x.Id == query.Id);

        if (entity is null)
        {
            return Result.NotFound();
        }

        return Result.Success(entity);
    }
}
