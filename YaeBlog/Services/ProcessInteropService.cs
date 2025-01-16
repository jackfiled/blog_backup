using System.Diagnostics;
using System.Runtime.InteropServices;
using YaeBlog.Core.Exceptions;

namespace YaeBlog.Services;

public class ProcessInteropService(ILogger<ProcessInteropService> logger)
{
    public Process StartProcess(string command, string arguments)
    {
        string commandName;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && !command.EndsWith(".exe"))
        {
            commandName = command + ".exe";
        }
        else
        {
            commandName = command;
        }

        try
        {
            ProcessStartInfo startInfo = new()
            {
                FileName = commandName,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process? process = Process.Start(startInfo);

            if (process is null)
            {
                throw new ProcessInteropException(
                    $"Failed to start process: {commandName}, the return process is null.");
            }

            process.OutputDataReceived += (_, data) =>
            {
                if (!string.IsNullOrEmpty(data.Data))
                {
                    logger.LogInformation("Receive output from process '{}': '{}'", commandName, data.Data);
                }
            };

            process.ErrorDataReceived += (_, data) =>
            {
                if (!string.IsNullOrEmpty(data.Data))
                {
                    logger.LogWarning("Receive error from process '{}': '{}'", commandName, data.Data);
                }
            };

            return process;
        }
        catch (Exception innerException)
        {
            throw new ProcessInteropException($"Failed to start process '{command}' with arguments '{arguments}",
                innerException);
        }
    }
}
