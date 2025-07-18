using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Milabowl.Snapshot;

public partial class SnapshotDelegatingHandler : DelegatingHandler
{
    private readonly string _snapshotDirectory;
    private readonly HashSet<string> _snapshotdUrls = new();

    public SnapshotDelegatingHandler(string snapshotDirectory)
    {
        _snapshotDirectory = snapshotDirectory;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var url = request.RequestUri?.ToString() ?? "unknown";
        if (!_snapshotdUrls.Add(url))
        {
            return await base.SendAsync(request, cancellationToken);
        }

        var response = await base.SendAsync(request, cancellationToken);
        if (response.Content.Headers.ContentType?.MediaType != "application/json")
        {
            return response;
        }

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var safeFileName = SanitizeFileName(url) + ".json";
        var filePath = Path.Combine(_snapshotDirectory, safeFileName);
        await File.WriteAllTextAsync(filePath, json, cancellationToken);
        Console.WriteLine($"Snapshot saved: {filePath}");
        return response;
    }

    private static string SanitizeFileName(string url)
    {
        return FilePathSanitizer().Replace(url, "_");
    }

    [GeneratedRegex("[\\/:*?\"<>|]")]
    private static partial Regex FilePathSanitizer();
}
