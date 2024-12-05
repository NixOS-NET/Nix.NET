namespace Nix;

using System.IO;

public class NixStoreEntry {
    public DirectoryInfo Path { get; }
    public string Hash => this.Path.Name.Substring(0, 32);
    public string Name => this.Path.Name.Substring(33);

    internal NixStoreEntry(DirectoryInfo dir) {
        this.Path = dir ?? throw new ArgumentNullException(nameof(dir));
    }

    public IAsyncEnumerable<NixStoreEntry> GetAllDependencies(CancellationToken cancel = default) {
        return NixStore.GetAllDependencies(this.Path, cancel);
    }

    public override string ToString() => this.Name;
}