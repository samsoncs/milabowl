namespace Milabowl.Processing.Utils
{
    public enum SnapshotMode
    {
        None,
        Read,
        Write,
    }

    public class FplApiOptions
    {
        public required string SnapshotPath { get; init; }
        public required SnapshotMode SnapshotMode { get; init; }
        public required int MainLeagueId { get; init; }
        public required int HeadToHeadLeagueId { get; init; }
    }
}
