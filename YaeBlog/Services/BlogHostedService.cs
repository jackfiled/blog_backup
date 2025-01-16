namespace YaeBlog.Services;

public class BlogHostedService(
    ILogger<BlogHostedService> logger,
    RendererService rendererService) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Welcome to YaeBlog!");

        await rendererService.RenderAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("YaeBlog stopped!\nHave a nice day!");
        return Task.CompletedTask;
    }
}
