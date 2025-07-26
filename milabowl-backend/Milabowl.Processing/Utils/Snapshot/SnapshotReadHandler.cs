namespace Milabowl.Processing.Utils.Snapshot;

public class SnapshotReadHandler : DelegatingHandler
{
    private readonly string _snapshotPath;

    public SnapshotReadHandler(ISnapshotPathResolver snapshotPathResolver)
    {
        _snapshotPath = snapshotPathResolver.GetSnapshotPath();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var url = request.RequestUri?.ToString() ?? "unknown";
        var safeFileName = SnapshotUrlSanitizer.SanitizeUrl(url) + ".json";
        var filePath = Path.Combine(_snapshotPath, safeFileName);
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Snapshot not found: {filePath}");
        }

        var json = await File.ReadAllTextAsync(filePath, cancellationToken);
        var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
        };
        return response;
    }
}
