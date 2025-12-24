using Ardalis.Result;
using Core;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace UseCases.GetAll;

public class GetTodosHandler : IQueryHandler<GetAllTodosQuery, Result<List<TodoEntity>>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetTodosHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<Result<List<TodoEntity>>> Handle(GetAllTodosQuery query, CancellationToken cancellationToken)
    {
        var list = await _dbContext.Todos.ToListAsync();
        return Result.Success(list);
    }
}
