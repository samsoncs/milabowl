namespace Milabowl.Processing.Utils;

public interface IFileSystem
{
    Task WriteAllTextAsync(string path, string? contents);
}

public class FileSystem: IFileSystem
{
    public Task WriteAllTextAsync(string path, string? contents)
    {
        return File.WriteAllTextAsync(path, contents);
    }
}
