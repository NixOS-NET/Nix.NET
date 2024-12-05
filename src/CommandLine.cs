namespace Nix;

class CommandLine {
    public static string Args(params string[] args) {
        return string.Join(" ", args.Select(arg => 
            // Escape special characters and wrap in quotes if needed
            arg.Contains(" ") || arg.Contains("\"") || arg.Contains("'") ? 
            $"\"{arg.Replace("\"", "\\\"")}\"" : arg
        ));
    }
}