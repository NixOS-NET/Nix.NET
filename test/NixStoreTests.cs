namespace Nix;

public class NixStoreTests {
    [SkippableFact]
    public async Task CurrentSystem_NullOnWindows() {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows), "Windows only");

        var system = await NixStore.GetCurrentSystem();
        Assert.Null(system);
    }

    [SkippableFact]
    public async Task GetAllDependencies() {
        var system = await NixStore.GetCurrentSystem();
        Skip.If(system is null, "NixOS only");

        var packages = system.GetAllDependencies().ToBlockingEnumerable().ToArray();
        Assert.NotEmpty(packages);

        var fuse3 = packages.FirstOrDefault(p => p.Name.StartsWith("fuse-3."));
    }
}