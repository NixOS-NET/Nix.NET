namespace Nix;

using System.IO;

public sealed class NixSystem: NixStoreEntry {
    internal NixSystem(DirectoryInfo dir): base(dir) {
    }
}