namespace Acelerador;

// DotnetRunner.cs
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

public class DotnetRunner
{
    private readonly string? _baseWorkingDirectory;

    public DotnetRunner(string? baseWorkingDirectory = null)
    {
        _baseWorkingDirectory = string.IsNullOrWhiteSpace(baseWorkingDirectory) ? null : baseWorkingDirectory;
    }

    public async Task<CommandResult> RunAsync(string arguments, string? workingDirectory = null, CancellationToken cancellationToken = default)
    {
        string? wd = workingDirectory ?? _baseWorkingDirectory;

        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = arguments,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
            WorkingDirectory = wd ?? Environment.CurrentDirectory
        };

        using var process = new Process { StartInfo = startInfo, EnableRaisingEvents = true };

        try
        {
            process.Start();

            // Leitura assíncrona evita bloqueios em saídas grandes
            var stdOutTask = process.StandardOutput.ReadToEndAsync();
            var stdErrTask = process.StandardError.ReadToEndAsync();

            await Task.WhenAll(stdOutTask, stdErrTask).ConfigureAwait(false);

            // Espera para garantir que o processo terminou
            await Task.Run(() => process.WaitForExit(), cancellationToken).ConfigureAwait(false);

            return new CommandResult
            {
                ExitCode = process.ExitCode,
                StdOut = await stdOutTask.ConfigureAwait(false),
                StdErr = await stdErrTask.ConfigureAwait(false)
            };
        }
        catch (OperationCanceledException)
        {
            if (!process.HasExited) try { process.Kill(); } catch { }
            throw;
        }
        catch (Exception ex)
        {
            return new CommandResult
            {
                ExitCode = -1,
                StdOut = string.Empty,
                StdErr = $"Exception: {ex.Message}"
            };
        }
    }
}
