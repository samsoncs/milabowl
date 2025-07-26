using System.Text.RegularExpressions;

namespace Milabowl.Processing.Utils.Snapshot;

public static partial class SnapshotUrlSanitizer
{
    public static string SanitizeUrl(string url)
    {
        return FilePathSanitizer().Replace(url, "_");
    }

    [GeneratedRegex(@"[\\\/:\*\?""<>|]")]
    private static partial Regex FilePathSanitizer();
}
