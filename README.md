[![NuGet](https://img.shields.io/nuget/v/Nix.svg)](https://www.nuget.org/packages/Nix/)

## NixStore

```csharp
var system = await NixStore.GetCurrentSystem();
if (system is null)
    throw new PlatformNotSupportedException("NixOS required");
var packages = await system.GetAllDependencies().ToListAsync();
Console.WriteLine($"Fuse3 installed: {packages.Any(p => p.Name.StartsWith("fuse-3."))}");
```