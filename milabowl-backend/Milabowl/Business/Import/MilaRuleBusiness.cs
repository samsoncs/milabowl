using System.Collections.Generic;
using Milabowl.Business.DTOs;
using System.Linq;
using Milabowl.Business.DTOs.Rules;
using Milabowl.Infrastructure.Models;

namespace Milabowl.Business.Import
{
    public interface IMilaRuleBusiness
    {
        decimal GetCapFailScore(IList<MilaRuleDTO> playerEvents);
        decimal GetBenchFailScore(IList<MilaRuleDTO> playerEvents);
        decimal GetCapKeepScore(IList<MilaRuleDTO> playerEvents);
        decimal GetCapDefScore(IList<MilaRuleDTO> playerEvents);
        decimal GetSixtyNine(IList<MilaRuleDTO> playerEvents);
        decimal GetRedCardScore(IList<MilaRuleDTO> playerEvents);
        decimal GetYellowCardScore(IList<MilaRuleDTO> playerEvents);
        decimal GetMinusIsPlusScore(IList<MilaRuleDTO> playerEvents);
        decimal GetTrendyBitchScore(IList<Player> subsIn, IList<Player> subsOut, Player mostTransferredInPlayer, Player mostTransferredOutPlayer);
        decimal GetSixtyNineSub(IList<MilaRuleDTO> playerEvents);
        decimal GetIncreaseStreakScore(IList<int> playerEvents, int gw);
        decimal GetHeadToHeadMetaScore(UserHeadToHeadDTO userHeadToHeadDto);
        decimal GetHeadToHeadStrongerOpponentScore(UserHeadToHeadDTO userHeadToHeadDto);
        decimal GetUniqueCaptainScore(Player currentUserCaptain, IList<Lineup> lineupsThisWeek);

        decimal GetGWScore(IList<MilaRuleDTO> playerEvents);
    }

    public class MilaRuleBusiness: IMilaRuleBusiness
    {
        
        public decimal GetCapFailScore(IList<MilaRuleDTO> playerEvents)
        {
            return playerEvents.Any(pe => pe.IsCaptain && pe.TotalPoints<5) ? -1 : 0;          //if cap_fail, return -1, otherwise 0
        }
        public decimal GetBenchFailScore(IList<MilaRuleDTO> playerEvents)
        {
            return (decimal) System.Math.Floor(playerEvents.Where(pe => pe.Multiplier==0).Sum(pe => pe.TotalPoints)/5.0) * (-1); 
        }

        public decimal GetCapKeepScore(IList<MilaRuleDTO> playerEvents){
            return playerEvents.Any(pe => pe.Multiplier == 2 && pe.PlayerPosition == 1) ? 2 : 0; //if goalkeeper, return 2
        }
        public decimal GetCapDefScore(IList<MilaRuleDTO> playerEvents){
            return playerEvents.Any(pe => pe.Multiplier == 2 && pe.PlayerPosition == 2) ? 1 : 0; //if defender, return 1
        }

        public decimal GetSixtyNine(IList<MilaRuleDTO> playerEvents)
        {
            decimal totalTeamScore = playerEvents.Sum(pe => pe.TotalPoints*pe.Multiplier);
            if(totalTeamScore == 69){
                return totalTeamScore/10;
            }
            return 0;
        }

        public decimal GetYellowCardScore(IList<MilaRuleDTO> playerEvents)
        {
            return playerEvents.Where(pe => pe.YellowCards==1).Sum(pe => pe.Multiplier);
        }
        
        public decimal GetRedCardScore(IList<MilaRuleDTO> playerEvents)
        {
            return playerEvents.Sum(pe => pe.RedCards*pe.Multiplier*3);
        }
        public decimal GetMinusIsPlusScore(IList<MilaRuleDTO> playerEvents)
        {
            return playerEvents.Where(pe => pe.TotalPoints<0).Sum(pe => pe.TotalPoints*(-1)*pe.Multiplier); 
        }

        public decimal GetTrendyBitchScore(IList<Player> subsIn, IList<Player> subsOut, Player mostTransferredInPlayer,
            Player mostTransferredOutPlayer)
        {
            return (mostTransferredInPlayer != null && subsIn.Any(p => p.PlayerId == mostTransferredInPlayer?.PlayerId) ? -1 :0)
                   + (mostTransferredOutPlayer != null && subsOut.Any(p => p.PlayerId == mostTransferredOutPlayer?.PlayerId) ? -1 : 0);
        }

        public decimal GetSixtyNineSub(IList<MilaRuleDTO> playerEvents)
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
            for(var i = 0; i < gw - 1; i++){
                if(pointsPerGameweek[i] < pointsPerGameweek[i+1]){
                    increaseStreakCount++;
                }
                else {
                    increaseStreakCount = 0;
                }
            }
            return increaseStreakCount > 1 ? 1 : 0;
        }

        public decimal GetHeadToHeadMetaScore(UserHeadToHeadDTO userHeadToHeadDto)
        {
            var scoreDiff = userHeadToHeadDto.UserPoints - userHeadToHeadDto.OpponentPoints;

            return userHeadToHeadDto.DidWin 
                   && scoreDiff is > 0 and <= 2 
                ? 2 : 0;
        }

        public decimal GetHeadToHeadStrongerOpponentScore(UserHeadToHeadDTO userHeadToHeadDto)
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

        public decimal GetGWScore(IList<MilaRuleDTO> playerEvents)
        {
            return playerEvents.Sum(pe => pe.TotalPoints*pe.Multiplier);
        }
    }
}
