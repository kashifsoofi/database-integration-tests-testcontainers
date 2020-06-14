namespace TodoApp.Infrastructure.Tests.Integration
{
    using System.Threading.Tasks;
    using Docker.DotNet;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using TestContainers.Container.Abstractions;
    using TestContainers.Container.Abstractions.Hosting;
    using TestContainers.Container.Abstractions.Images;

    public class MigrationsContainer : GenericContainer
    {
        internal IDockerClient DockerClient { get; }

        private static IImage CreateDefaultImage(IDockerClient dockerClient, ILoggerFactory loggerFactory)
        {
            return new ImageBuilder<DockerfileImage>()
                .ConfigureImage((context, image) =>
                {
                    image.DockerfilePath = "Dockerfile";
                    image.DeleteOnExit = true;
                    image.BasePath = "../../../../../db";
                })
                .Build();
        }

        [ActivatorUtilitiesConstructor]
        public MigrationsContainer(IDockerClient dockerClient, ILoggerFactory loggerFactory)
            : base(CreateDefaultImage(dockerClient, loggerFactory), dockerClient, loggerFactory)
        {
            this.DockerClient = dockerClient;
        }

        public async Task<long> GetExitCodeAsync()
        {
            var containerWaitResponse = await this.DockerClient.Containers.WaitContainerAsync(this.ContainerId);
            return containerWaitResponse.StatusCode;
        }
    }
}
