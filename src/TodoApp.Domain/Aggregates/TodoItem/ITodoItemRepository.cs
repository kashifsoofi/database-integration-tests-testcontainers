namespace TodoApp.Domain.Aggregates.TodoItem
{
    using System.Threading.Tasks;

    public interface ITodoItemRepository
    {
        Task CreateAsync(TodoItem item);

        Task UpdateAsync(TodoItem item);

        Task<TodoItem> GetByIdAsync(int id);
    }
}
