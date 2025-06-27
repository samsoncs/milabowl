using Milabowl.Processing.DataImport.FplDtos;
using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.DataImport;

public static class FplMapperExtensions
{
    public static HeadToHead ToHeadToHeadEvent(
        this HeadToHeadEventRootDto headToHeadEventRootDto,
        int userEntry
    )
    {
        var headToHeadHomeDto = headToHeadEventRootDto.Results.FirstOrDefault(r =>
            r.Entry1Entry == userEntry
        );

        if (headToHeadHomeDto is not null)
        {
            return new HeadToHead(
                GetHeadToHeadEvent(headToHeadHomeDto, true),
                GetHeadToHeadEvent(headToHeadHomeDto, false)
            );
        }

        var headToHeadAwayDto = headToHeadEventRootDto.Results.First(r =>
            r.Entry2Entry == userEntry
        );

        return new HeadToHead(
            GetHeadToHeadEvent(headToHeadAwayDto, false),
            GetHeadToHeadEvent(headToHeadAwayDto, true)
        );
    }

    private static HeadToHeadEvent GetHeadToHeadEvent(
        HeadToHeadResultDto headToHeadResultDto,
        bool isEntryOne
    )
    {
        var entryId = isEntryOne
            ? headToHeadResultDto.Entry1Entry
            : headToHeadResultDto.Entry2Entry;
        return new HeadToHeadEvent(
            isEntryOne ? headToHeadResultDto.Entry1Points : headToHeadResultDto.Entry2Points,
            isEntryOne
                ? headToHeadResultDto.Entry1Win == 1
                : headToHeadResultDto.Entry2Win == 1,
            isEntryOne
                ? headToHeadResultDto.Entry1Draw == 1
                : headToHeadResultDto.Entry2Draw == 1,
            isEntryOne
                ? headToHeadResultDto.Entry1Loss == 1
                : headToHeadResultDto.Entry2Loss == 1,
            isEntryOne ? headToHeadResultDto.Entry1Total : headToHeadResultDto.Entry2Total,
            headToHeadResultDto.IsKnockout,
            headToHeadResultDto.League,
            headToHeadResultDto.IsBye,
            entryId
        );
    }

    public static Event ToEvent(this EventDto @event)
    {
        return new Event(@event.Id, @event.Name);
    }

    public static User ToUser(this ResultDto user)
    {
        return new User(
            user.Id,
            user.Entry,
            user.PlayerName,
            user.EntryName,
            user.Rank,
            user.LastRank,
            user.EventTotal
        );
    }

    public static IList<PlayerEvent> ToLineup(
        this PicksRootDto picksRootDto,
        EventRootDto eventRootDto,
        List<PlayerDto> players,
        List<TeamDto> teams
    )
    {
        return picksRootDto
            .Picks.Select(p =>
            {
                var element = eventRootDto.Elements.First(e => e.Id == p.Element);
                var player = players.First(plr => plr.Id == element.Id);
                var team = teams.First(t => t.Id == player.Team);
                return element.ToPlayerEvent(player, team, p);
            })
            .ToList();
    }

    private static PlayerEvent ToPlayerEvent(this ElementDto element,
        PlayerDto player,
        TeamDto team,
        PickDto pick
    )
    {
        return new PlayerEvent(
            player.FirstName,
            player.SecondName,
            player.WebName,
            element.Id,
            team.Id,
            team.Code,
            team.Name,
            team.ShortName,
            element.Stats.Minutes,
            element.Stats.GoalsScored,
            element.Stats.Assists,
            element.Stats.GoalsConceded,
            element.Stats.CleanSheets,
            element.Stats.PenaltiesSaved,
            element.Stats.OwnGoals,
            element.Stats.PenaltiesMissed,
            element.Stats.YellowCards,
            element.Stats.RedCards,
            element.Stats.Saves,
            element.Stats.Bonus,
            element.Stats.Bps,
            element.Stats.Influence,
            element.Stats.Creativity,
            element.Stats.Threat,
            element.Stats.IctIndex,
            element.Stats.TotalPoints,
            element.Stats.InDreamteam,
            pick.Multiplier,
            pick.IsCaptain,
            pick.IsViceCaptain,
            player.ElementType switch
            {
                1 => PlayerPosition.GK,
                2 => PlayerPosition.DEF,
                3 => PlayerPosition.MID,
                4 => PlayerPosition.MID,
                5 => PlayerPosition.MAN,
                _ => throw new ArgumentOutOfRangeException()
            },
            player.ElementType switch
            {
                1 => "GK",
                2 => "DEF",
                3 => "MID",
                4 => "FWD",
                5 => "MAN",
                _ => throw new ArgumentOutOfRangeException()
            }
        );
    }
}
