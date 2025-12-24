using Core;

namespace TodoApi.Endpoints.Responses
{
    public record TodoResponse(Guid Id, string Name)
    {
        public static TodoResponse FromEntity(TodoEntity entity)
        {
            return new(entity.Id.Value, entity.Name);
        }
    }
}
