namespace Nix;

using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public static class NixStore {
    public static async Task<NixSystem?> GetCurrentSystem(CancellationToken cancel = default) {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return null;

        var systemDir = new DirectoryInfo("/run/current-system");
        return systemDir.Exists ? new(systemDir) : null;
    }

    public static async IAsyncEnumerable<NixStoreEntry> GetAllDependencies(
        DirectoryInfo storePath, [EnumeratorCancellation] CancellationToken cancel = default) {
        if (storePath is null)
            throw new ArgumentNullException(nameof(storePath));

        await foreach (string path in Run("nix-store",
                                          ["--query", "--requisites", storePath.FullName], cancel)
                           .ConfigureAwait(false)) {
            if (string.IsNullOrEmpty(path))
                continue;

            yield return new(new DirectoryInfo(path));
        }
    }

    internal static async IAsyncEnumerable<string> Run(string command, string[] args,
                                                       [EnumeratorCancellation]
                                                       CancellationToken cancel = default) {
#if NET8_0_OR_GREATER
        var psi = new ProcessStartInfo(command) {
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };
        foreach (string arg in args)
            psi.ArgumentList.Add(arg);
#else
        var psi = new ProcessStartInfo(command, CommandLine.Args(args)) {
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };
#endif

        using var process = Process.Start(psi)
                         ?? throw new NotSupportedException("Failed to start process");
        while (true) {
            cancel.ThrowIfCancellationRequested();
#if NET8_0_OR_GREATER
            string? line = await process.StandardOutput.ReadLineAsync(cancel)
                                        .ConfigureAwait(false);
#else
            string? line = await process.StandardOutput.ReadLineAsync().ConfigureAwait(false);
#endif
            if (line is null)
                break;

            yield return line;
        }
    }
}