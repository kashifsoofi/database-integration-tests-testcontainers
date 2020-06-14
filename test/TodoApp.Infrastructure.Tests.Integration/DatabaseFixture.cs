namespace TodoApp.Infrastructure.Tests.Integration
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TestContainers.Container.Abstractions.Hosting;
    using TestContainers.Container.Database.Hosting;
    using TestContainers.Container.Database.MySql;
    using Xunit;

    public class DatabaseFixture : IDisposable, IAsyncLifetime
    {
        private readonly MySqlContainer databaseContainer;
        private MigrationsContainer migrationsContainer;

        private bool RunningInDocker => Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

        private string Server => RunningInDocker ? "host.docker.internal" : "localhost";

        public string ConnectionString => $"Server={this.databaseContainer.GetDockerHostIpAddress()};Port={this.databaseContainer.GetMappedPort(MySqlContainer.DefaultPort)};database=Todo;uid={this.databaseContainer.Username};password={this.databaseContainer.Password};SslMode=None;";

        public DatabaseFixture()
        {
            Environment.SetEnvironmentVariable("REAPER_DISABLED", true.ToString());
            this.databaseContainer = new ContainerBuilder<MySqlContainer>()
                .ConfigureDockerImageName("mysql:5.6")
                .ConfigureDatabaseConfiguration("root", "integration123", "defaultdb")
                .Build();
        }

        public void Dispose()
        {
        }

        public async Task InitializeAsync()
        {
            await this.databaseContainer.StartAsync();

            this.migrationsContainer = new ContainerBuilder<MigrationsContainer>()
                .ConfigureContainer((context, container) =>
                {
                    var connectionString = $"Server=host.docker.internal;Port={this.databaseContainer.GetMappedPort(MySqlContainer.DefaultPort)};database={this.databaseContainer.DatabaseName};uid={this.databaseContainer.Username};password={this.databaseContainer.Password};SslMode=None;";
                    container.Command = new List<string> { "-cs", connectionString };
                })
                .Build();

            await this.migrationsContainer.StartAsync();
            var exitCode = await this.migrationsContainer.GetExitCodeAsync();
            if (exitCode > 0)
            {
                throw new Exception("Database migrations failed");
            }
        }

        public async Task DisposeAsync()
        {
            if (this.migrationsContainer != null)
            {
                await this.migrationsContainer.StopAsync();
            }

            if (this.databaseContainer != null)
            {
                await this.databaseContainer.StopAsync();
            }
        }
    }

    [CollectionDefinition("Database Collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
