using System.Diagnostics;
using Microsoft.Extensions.Options;
using YaeBlog.Models;

namespace YaeBlog.Services;

/// <summary>
/// 在应用程序运行的过程中启动Tailwind watch
/// 在程序退出时自动结束进程
/// 只在Development模式下启动
/// </summary>
public sealed class TailwindRefreshService(
    IOptions<TailwindOptions> options,
    ProcessInteropService processInteropService,
    IHostEnvironment hostEnvironment,
    ILogger<TailwindRefreshService> logger) : IHostedService, IDisposable
{
    private Process? _tailwindProcess;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (!hostEnvironment.IsDevelopment())
        {
            return Task.CompletedTask;
        }

        logger.LogInformation("Try to start tailwind watcher with input {} and output {}", options.Value.InputFile,
            options.Value.OutputFile);

        _tailwindProcess = processInteropService.StartProcess("pnpm",
            $"tailwind -i {options.Value.InputFile} -o {options.Value.OutputFile} --watch");

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _tailwindProcess?.Kill();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _tailwindProcess?.Dispose();
    }
}
