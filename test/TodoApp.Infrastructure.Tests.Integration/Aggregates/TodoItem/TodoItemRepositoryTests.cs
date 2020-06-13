namespace TodoApp.Infrastructure.Tests.Integration.Aggregates.TodoItem
{
    using System.Threading.Tasks;
    using AutoFixture.Xunit2;
    using TodoApp.Infrastructure.Aggregates.TodoItem;
    using Xunit;
    using TodoApp.Domain.Aggregates.TodoItem;
    using FluentAssertions;

    [Collection("Database Collection")]
    public class TodoItemRepositoryTests : IAsyncLifetime
    {
        private readonly TodoItemDatabaseHelper todoItemDatabaseHelper;

        private readonly TodoItemRepository sut;

        public TodoItemRepositoryTests(DatabaseFixture databaseFixture)
        {
            this.todoItemDatabaseHelper = new TodoItemDatabaseHelper(databaseFixture.ConnectionString);

            this.sut = new TodoItemRepository(databaseFixture.ConnectionString);
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            await todoItemDatabaseHelper.CleanupAsync();
        }

        [Theory]
        [AutoData]
        public async Task CreateAsync_ShouldCreateTodoItem(TodoItem itemToCreate)
        {
            // arrange

            // act
            await this.sut.CreateAsync(itemToCreate);

            // assert
            var createdItem = await this.todoItemDatabaseHelper.GetByIdAsync(itemToCreate.Id);
            createdItem.Should().NotBeNull();
            createdItem.Name.Should().Be(itemToCreate.Name);
            createdItem.Status.Should().Be(itemToCreate.Status);
        }

        [Theory]
        [AutoData]
        public async Task UpdateAsync_ShouldUpdateTodoItem(TodoItem item, TodoItem itemToUpdate)
        {
            // arrange
            await this.todoItemDatabaseHelper.CreateAsync(item);
            itemToUpdate.Id = item.Id;

            // act
            await this.sut.UpdateAsync(itemToUpdate);

            // assert
            var updatedItem = await this.todoItemDatabaseHelper.GetByIdAsync(itemToUpdate.Id);
            updatedItem.Should().NotBeNull();
            updatedItem.Name.Should().Be(itemToUpdate.Name);
            updatedItem.Status.Should().Be(itemToUpdate.Status);
        }

        [Theory]
        [AutoData]
        public async Task GetByIdAsync_GivenRecordDoesNotExist_ShouldReturnNull(int id)
        {
            // arrange

            // act
            var item = await this.sut.GetByIdAsync(id);

            // assert
            item.Should().BeNull();
        }

        [Theory]
        [AutoData]
        public async Task GetByIdAsync_GivenRecordExist_ShouldReturnTodoItem(TodoItem expectedItem)
        {
            // arrange
            await this.todoItemDatabaseHelper.CreateAsync(expectedItem);

            // act
            var item = await this.sut.GetByIdAsync(expectedItem.Id);

            // assert
            item.Should().NotBeNull();
            item.Name.Should().Be(expectedItem.Name);
            item.Status.Should().Be(expectedItem.Status);
        }
    }
}
