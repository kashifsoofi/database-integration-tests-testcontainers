namespace TodoApp.Infrastructure.Tests.Integration
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dapper;
    using MySql.Data.MySqlClient;
    using TodoApp.Domain.Aggregates.TodoItem;

    public class TodoItemDatabaseHelper
    {
        private readonly string connectionString;
        private readonly List<int> trackedIds = new List<int>();

        public TodoItemDatabaseHelper(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task CreateAsync(TodoItem item)
        {
            var sql = "INSERT INTO TodoItem (Id, CreatedOn, UpdatedOn, Name, Status) VALUES (@Id, @CreatedOn, @UpdatedOn, @Name, @Status)";
            using (var connection = new MySqlConnection(this.connectionString))
            {
                await connection.ExecuteAsync(sql, item);
                this.trackedIds.Add(item.Id);
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

        public async Task CleanupAsync()
        {
            if (this.trackedIds.Count > 0)
            {
                var sql = "DELETE FROM TodoItem WHERE Id = @id";
                using (var connection = new MySqlConnection(this.connectionString))
                {
                    foreach (var id in this.trackedIds)
                    {
                        await connection.ExecuteAsync(sql, new { id });
                    }
                }
            }
        }
    }
}
