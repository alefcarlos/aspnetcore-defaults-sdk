using Core;
using Microsoft.EntityFrameworkCore;

namespace UseCases;

public interface IApplicationDbContext
{
    DbSet<TodoEntity> Todos { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}