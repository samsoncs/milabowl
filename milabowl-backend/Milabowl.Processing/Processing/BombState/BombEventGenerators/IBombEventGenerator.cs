using Milabowl.Processing.Processing.BombState.Models;

namespace Milabowl.Processing.Processing.BombState.BombEventGenerators;

public interface IBombEventGenerator
{
    bool CanGenerate(ManagerBombState bombState);
    BombHistoryRow Generate(ManagerBombState bombState);
    IList<BombStateDisplayEmoji> GenerateDisplayEmojis(ManagerBombState bombState);
}
