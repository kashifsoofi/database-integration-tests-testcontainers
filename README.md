# Database Integration Tests with TestContainers DotNet
This is a sample dotnet project to run database integration tests by starting up database server as Docker Container. There is also a second docker container to apply database migrations using [RoundHouseE](https://github.com/chucknorris/roundhouse).

## Database Access
[TodoItemRepository](https://github.com/kashifsoofi/database-integration-tests-testcontainers/blob/master/src/TodoApp.Infrastructure/Aggregates/TodoItem/TodoItemRepository.cs) is sample implementation of database access using [MySqlConnector](https://github.com/mysql-net/MySqlConnector) and [Dapper](https://github.com/StackExchange/Dapper).

## Integration Tests
[TodoItemRepositoryTests](https://github.com/kashifsoofi/database-integration-tests-testcontainers/blob/master/test/TodoApp.Infrastructure.Tests.Integration/Aggregates/TodoItem/TodoItemRepositoryTests.cs) implements integration tests for the repository. Tests are decorated with XUnit's [collection-fixture](https://xunit.net/docs/shared-context.html#collection-fixture) to initialise database before any tests in `Database Collection` are executed.

### DatabaseFixture
`DatabaseFixture` makes use of [TestContainers DotNet](https://github.com/isen-ng/testcontainers-dotnet). It builds a database container in our case MySql version 5.6 in constructor.  
In `InitializeAsync` we run the contianer, then builds a new image for database migrations and creates a new container from that image and waits for it to exit, if exit code is 0 migrations are applied if > 0 migrations are failed so we can just exit from here.  

All the integrations tests are executed after the migrations are successfully applied.  

After all integration tests are executed `DisposeAsync` stops bothe migrations and database containers.  

## Cake Build
Execute following command to run cake on host machine
```
dotnet cake
```
Execute following command to run cake in docker container
```
docker run -v /Users/kashif/Projects/GitHub/database-integration-tests-testcontainers:/src -v /var/run/docker.sock:/var/run/docker.sock --rm --workdir /src/cake cake:1.0
```

## References & Resources
* [TestContainers DotNet](https://github.com/isen-ng/testcontainers-dotnet)  
* [MySqlConnector](https://github.com/mysql-net/MySqlConnector)
* [Dapper](https://github.com/StackExchange/Dapper)  
* [RoundHouseE](https://github.com/chucknorris/roundhouse)  
* [collection-fixture](https://xunit.net/docs/shared-context.html#collection-fixture)  

