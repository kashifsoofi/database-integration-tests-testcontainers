# Database Integration Tests with TestContainers DotNet

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
