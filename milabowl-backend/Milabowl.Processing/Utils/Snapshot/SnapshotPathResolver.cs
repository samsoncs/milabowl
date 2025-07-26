using Microsoft.Extensions.Options;

namespace Milabowl.Processing.Utils.Snapshot;

public interface ISnapshotPathResolver
{
    string GetSnapshotPath();
}

public class SnapshotPathResolver : ISnapshotPathResolver
{
    private readonly string _resolvedPath;

    public SnapshotPathResolver(IOptions<FplApiOptions> options)
    {
        _resolvedPath = ResolveSnapshotPath(options.Value.SnapshotPath);
    }

    public string GetSnapshotPath() => _resolvedPath;

    private string ResolveSnapshotPath(string relativeSnapshotPath)
    {
        var solutionPath = FindSolutionPath();
        if (solutionPath == null)
        {
            throw new FileNotFoundException("Could not find root directory of Milabowl backend");
        }
        var solutionDir = Path.GetDirectoryName(solutionPath)!;
        return Path.GetFullPath(Path.Combine(solutionDir, relativeSnapshotPath));
    }

    private static string? FindSolutionPath()
    {
        var dir = AppContext.BaseDirectory;
        while (!string.IsNullOrEmpty(dir))
        {
            var candidate = Path.Combine(dir, "milabowl.sln");
            if (File.Exists(candidate))
                return candidate;
            var parent = Directory.GetParent(dir);
            dir = parent?.FullName;
        }
        return null;
    }
}
