namespace Milabowl.Processing.Utils;

public interface IFilePathResolver
{
    string ResolveGameStateFilePath();
}

public class FilePathResolver : IFilePathResolver
{
    public string ResolveGameStateFilePath()
    {
        var gitRoot = FindGitRoot(AppContext.BaseDirectory);
        var gameStatePath = Path.Combine(gitRoot, "milabowl-astro", "src", "game_state");
        return gameStatePath;
    }

    private string FindGitRoot(string startDir)
    {
        var dir = new DirectoryInfo(startDir);
        while (dir != null && !File.Exists(Path.Combine(dir.FullName, ".git", "config")))
        {
            dir = dir.Parent;
        }
        return dir?.FullName ?? throw new DirectoryNotFoundException("Git root not found.");
    }
}
