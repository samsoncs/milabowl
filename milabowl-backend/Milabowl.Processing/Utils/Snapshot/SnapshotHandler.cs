namespace Milabowl.Processing.Utils.Snapshot;

public class SnapshotHandler : DelegatingHandler
{
    private readonly string _snapshotPath;
    private readonly HashSet<string> _snapshotdUrls = new();

    public SnapshotHandler(ISnapshotPathResolver snapshotPathResolver)
    {
        _snapshotPath = snapshotPathResolver.GetSnapshotPath();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var url = request.RequestUri?.ToString() ?? "unknown";
        if (!_snapshotdUrls.Add(url))
        {
            return await base.SendAsync(request, cancellationToken);
        }

        var safeFileName = SnapshotUrlSanitizer.SanitizeUrl(url) + ".json";
        var filePath = Path.Combine(_snapshotPath, safeFileName);
        if (!Directory.Exists(_snapshotPath))
        {
            Directory.CreateDirectory(_snapshotPath);
        }
        var response = await base.SendAsync(request, cancellationToken);
        if (response.Content.Headers.ContentType?.MediaType != "application/json")
        {
            return response;
        }

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        await File.WriteAllTextAsync(filePath, json, cancellationToken);
        Console.WriteLine($"Snapshot saved: {filePath}");
        return response;
    }
}
