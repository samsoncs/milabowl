﻿using Milabowl.Domain.Entities.Fantasy;

namespace Milabowl.Domain.Processing
{
    public interface IMilaRuleBusiness
    {
        decimal GetCapFailScore(IList<MilaRuleData> playerEvents);
        decimal GetBenchFailScore(IList<MilaRuleData> playerEvents);
        decimal GetCapKeepScore(IList<MilaRuleData> playerEvents);
        decimal GetCapDefScore(IList<MilaRuleData> playerEvents);
        decimal GetSixtyNine(IList<MilaRuleData> playerEvents);
        decimal GetRedCardScore(IList<MilaRuleData> playerEvents);
        decimal GetYellowCardScore(IList<MilaRuleData> playerEvents);
        decimal GetMinusIsPlusScore(IList<MilaRuleData> playerEvents);
        decimal GetTrendyBitchScore(IList<Player> subsIn, IList<Player> subsOut, Player mostTransferredInPlayer, Player mostTransferredOutPlayer);
        decimal GetSixtyNineSub(IList<MilaRuleData> playerEvents);
        decimal GetIncreaseStreakScore(IList<int> playerEvents, int gw);
        decimal GetHeadToHeadMetaScore(UserHeadToHead userHeadToHeadDto);
        decimal GetHeadToHeadStrongerOpponentScore(UserHeadToHead userHeadToHeadDto);
        decimal GetUniqueCaptainScore(Player currentUserCaptain, IList<Lineup> lineupsThisWeek);

        decimal GetGWScore(IList<MilaRuleData> playerEvents);
    }

    public class MilaRuleBusiness : IMilaRuleBusiness
    {

        public decimal GetCapFailScore(IList<MilaRuleData> playerEvents)
        {
            return playerEvents.Any(pe => pe.IsCaptain && pe.TotalPoints < 5) ? -1 : 0;          //if cap_fail, return -1, otherwise 0
        }
        public decimal GetBenchFailScore(IList<MilaRuleData> playerEvents)
        {
            return (decimal)Math.Floor(playerEvents.Where(pe => pe.Multiplier == 0).Sum(pe => pe.TotalPoints) / 5.0) * -1;
        }

        public decimal GetCapKeepScore(IList<MilaRuleData> playerEvents)
        {
            return 0;//playerEvents.Any(pe => pe.Multiplier == 2 && pe.PlayerPosition == 1) ? 2 : 0; //if goalkeeper, return 2
        }
        public decimal GetCapDefScore(IList<MilaRuleData> playerEvents)
        {
            return playerEvents.Any(pe => pe.Multiplier == 2 && pe.PlayerPosition == 2) ? 1 : 0; //if defender, return 1
        }

        public decimal GetSixtyNine(IList<MilaRuleData> playerEvents)
        {
            decimal totalTeamScore = playerEvents.Sum(pe => pe.TotalPoints * pe.Multiplier);
            if (totalTeamScore == 69)
            {
                return totalTeamScore / 10;
            }
            return 0;
        }

        public decimal GetYellowCardScore(IList<MilaRuleData> playerEvents)
        {
            return playerEvents.Where(pe => pe.YellowCards == 1).Sum(pe => pe.Multiplier);
        }

        public decimal GetRedCardScore(IList<MilaRuleData> playerEvents)
        {
            return playerEvents.Sum(pe => pe.RedCards * pe.Multiplier * 3);
        }
        public decimal GetMinusIsPlusScore(IList<MilaRuleData> playerEvents)
        {
            return playerEvents.Where(pe => pe.TotalPoints < 0).Sum(pe => pe.TotalPoints * -1 * pe.Multiplier);
        }

        public decimal GetTrendyBitchScore(IList<Player> subsIn, IList<Player> subsOut, Player mostTransferredInPlayer,
            Player mostTransferredOutPlayer)
        {
            return (mostTransferredInPlayer != null && subsIn.Any(p => p.PlayerId == mostTransferredInPlayer?.PlayerId) ? -1 : 0)
                   + (mostTransferredOutPlayer != null && subsOut.Any(p => p.PlayerId == mostTransferredOutPlayer?.PlayerId) ? -1 : 0);
        }

        public decimal GetSixtyNineSub(IList<MilaRuleData> playerEvents)
        {
            var cap = playerEvents.FirstOrDefault(pe => pe.IsCaptain);

            if (cap is { Minutes: 69 })
            {
                return 2.69m * cap.Multiplier;

            }

            return playerEvents.Any(pe => pe.Minutes == 69) ? 2.69m : 0;
        }

        public decimal GetIncreaseStreakScore(IList<int> pointsPerGameweek, int gw)
        {

            int increaseStreakCount = 0;
            for (var i = 0; i < gw - 1; i++)
            {
                if (pointsPerGameweek[i] < pointsPerGameweek[i + 1])
                {
                    increaseStreakCount++;
                }
                else
                {
                    increaseStreakCount = 0;
                }
            }
            return increaseStreakCount > 1 ? 1 : 0;
        }

        public decimal GetHeadToHeadMetaScore(UserHeadToHead userHeadToHeadDto)
        {
            var scoreDiff = userHeadToHeadDto.UserPoints - userHeadToHeadDto.OpponentPoints;

            return userHeadToHeadDto.DidWin
                   && scoreDiff is > 0 and <= 2
                ? 2 : 0;
        }

        public decimal GetHeadToHeadStrongerOpponentScore(UserHeadToHead userHeadToHeadDto)
        {
            return userHeadToHeadDto.DidWin ? 1 : 0;
        }

        public decimal GetUniqueCaptainScore(Player currentUserCaptain, IList<Lineup> lineupsThisWeek)
        {
            return currentUserCaptain != null && lineupsThisWeek.SelectMany(l => l.PlayerEventLineups)
                .Where(pel => pel.IsCaptain)
                .Count(pel => pel.PlayerEvent.FkPlayerId == currentUserCaptain?.PlayerId)
                == 1 ? 2 : 0;
        }

        public decimal GetGWScore(IList<MilaRuleData> playerEvents)
        {
            return playerEvents.Sum(pe => pe.TotalPoints * pe.Multiplier);
        }
    }
}