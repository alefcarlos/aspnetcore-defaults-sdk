using Microsoft.EntityFrameworkCore;
using Core;

namespace Infra.EntityConfigurations;

public class TodoEntityConfiguration : IEntityTypeConfiguration<TodoEntity>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<TodoEntity> builder)
    {
        builder.ToTable("Todos");
    }
}