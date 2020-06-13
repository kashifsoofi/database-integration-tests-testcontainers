namespace TodoApp.Domain.Aggregates.TodoItem
{
    using System;

    public class TodoItem
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string Name { get; set; }

        public ItemStatus Status { get; set; }
    }
}
