namespace TodoApp.Infrastructure.Tests.Integration
{
    using System;
    using Xunit;

    public class DatabaseFixture : IDisposable
    {
        private bool RunningInDocker => Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

        private string Server => RunningInDocker ? "host.docker.internal" : "localhost";

        public string ConnectionString => $"server={Server};Port=33061;database=Todo;uid=root;password=Password123;SslMode=None;";

        public void Dispose()
        {
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
