using Milabowl.Processing.Processing.BombState.Models;

namespace Milabowl.Processing.Processing.BombState.CurrentBombEmojiGenerators;

public interface ICurrentBombEmojiGenerator
{
    bool CanGenerate(ManagerBombState bombState);
    IList<BombStateDisplayEmoji> Generate(ManagerBombState bombState);
}
