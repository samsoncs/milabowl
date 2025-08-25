using Milabowl.Processing.Processing.BombState.Models;

namespace Milabowl.Processing.Processing.BombState;

public static class BombHelper
{
    public static string GetTierName(BombTier bombTier)
    {
        return bombTier switch
        {
            BombTier.Dynamite => "dynamite",
            BombTier.Bomb => "bomb",
            BombTier.Nuke => "nuke",
            _ => "dynamite"
        };
    }

    public static string GetCapitalizedTierName(BombTier bombTier)
    {
        return bombTier switch
        {
            BombTier.Dynamite => "Dynamite",
            BombTier.Bomb => "Bomb",
            BombTier.Nuke => "Nuke",
            _ => "Dynamite"
        };
    }

    public static int GetBombPoints(BombTier bombTier)
    {
        return bombTier switch
        {
            BombTier.Dynamite => -2,
            BombTier.Bomb => -4,
            BombTier.Nuke => -6,
            _ => -2
        };
    }

    private const string Dynamite = "🧨";
    private const string Bomb = "💣";
    private const string Nuke = "☢️";
    public const string Exploded = "💥";
    public const string Collateral = "💀";
    public const string Thrown = "🤾";
    public const string Diffused = "💨";
    public const string DiffusalKit = "🧰";
    public const string Holding = "👐";

    public static string GetBombEmoji(BombTier bombTier)
    {
        return bombTier switch
        {
            BombTier.Dynamite => Dynamite,
            BombTier.Bomb => Bomb,
            BombTier.Nuke => Nuke,
            _ => Dynamite
        };
    }
}
