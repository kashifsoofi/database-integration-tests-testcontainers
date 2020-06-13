namespace TodoApp.Infrastructure.Aggregates.TodoItem
{
    using System.Threading.Tasks;
    using MySql.Data.MySqlClient;
    using TodoApp.Domain.Aggregates.TodoItem;
    using Dapper;

    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly string connectionString;

        public TodoItemRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task CreateAsync(TodoItem item)
        {
            var sql = "INSERT INTO TodoItem (Id, CreatedOn, UpdatedOn, Name, Status) VALUES (@Id, @CreatedOn, @UpdatedOn, @Name, @Status)";
            using (var connection = new MySqlConnection(this.connectionString))
            {
                await connection.ExecuteAsync(sql, item);
            }
        }

        public async Task UpdateAsync(TodoItem item)
        {
            var sql = "UPDATE TodoItem SET Id = @Id, UpdatedOn = @UpdatedOn, Name = @Name, Status = @Status WHERE Id = @Id";
            using (var connection = new MySqlConnection(this.connectionString))
            {
                await connection.ExecuteAsync(sql, item);
            }
        }

        public async Task<TodoItem> GetByIdAsync(int id)
        {
            var sql = "SELECT Id, CreatedOn, UpdatedOn, Name, Status FROM TodoItem WHERE Id = @id";
            using (var connection = new MySqlConnection(this.connectionString))
            {
                var todoItem = await connection.QueryFirstOrDefaultAsync<TodoItem>(sql, new { id });
                return todoItem;
            }
        }
    }
}
