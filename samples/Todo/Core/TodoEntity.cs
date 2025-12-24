namespace Core;

public readonly record struct TodoId(Guid Value)
{
    public static TodoId Empty { get; } = new(Guid.Empty);
    public static TodoId NewTodoId() => new(Guid.NewGuid());
}

public class TodoEntity
{
    public TodoId Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    private TodoEntity() { }

    public static TodoEntity CreateNew(string name)
    {
        var entity = new TodoEntity
        {
            Id = TodoId.NewTodoId(),
            Name = name
        };

        return entity;
    }
}
