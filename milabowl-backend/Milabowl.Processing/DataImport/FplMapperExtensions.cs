using System.Diagnostics;
using Milabowl.Processing.DataImport.FplDtos;
using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.DataImport;

public static class FplMapperExtensions
{
    public static HeadToHead ToHeadToHeadEvent(
        this HeadToHeadEventRootDTO headToHeadEventRootDto,
        int userEntry
    )
    {
        var headToHeadHomeDto = headToHeadEventRootDto.results.FirstOrDefault(r =>
            r.entry_1_entry == userEntry
        );

        if (headToHeadHomeDto is not null)
        {
            return new HeadToHead(
                GetHeadToHeadEvent(headToHeadHomeDto, true),
                GetHeadToHeadEvent(headToHeadHomeDto, false)
            );
        }

        var headToHeadAwayDto = headToHeadEventRootDto.results.First(r =>
            r.entry_2_entry == userEntry
        );

        return new HeadToHead(
            GetHeadToHeadEvent(headToHeadAwayDto, false),
            GetHeadToHeadEvent(headToHeadAwayDto, true)
        );
    }

    private static HeadToHeadEvent GetHeadToHeadEvent(
        HeadToHeadResultDTO headToHeadResultDto,
        bool isEntryOne
    )
    {
        return new HeadToHeadEvent(
            isEntryOne ? headToHeadResultDto.entry_1_points : headToHeadResultDto.entry_2_points,
            isEntryOne
                ? headToHeadResultDto.entry_1_win == 1
                : headToHeadResultDto.entry_2_win == 1,
            isEntryOne
                ? headToHeadResultDto.entry_1_draw == 1
                : headToHeadResultDto.entry_2_draw == 1,
            isEntryOne
                ? headToHeadResultDto.entry_1_loss == 1
                : headToHeadResultDto.entry_2_loss == 1,
            isEntryOne ? headToHeadResultDto.entry_1_total : headToHeadResultDto.entry_2_total,
            headToHeadResultDto.is_knockout,
            headToHeadResultDto.league,
            headToHeadResultDto.is_bye
        );
    }

    public static Event ToEvent(this EventDTO @event)
    {
        return new Event(@event.Id, @event.Name);
    }

    public static User ToUser(this ResultDTO user)
    {
        return new User(
            user.id,
            user.player_name,
            user.entry_name,
            user.rank,
            user.last_rank,
            user.event_total
        );
    }

    public static IList<PlayerEvent> ToLineup(
        this PicksRootDTO picksRootDto,
        EventRootDTO eventRootDto,
        List<PlayerDTO> players
    )
    {
        return picksRootDto
            .picks.Select(p =>
            {
                var element = eventRootDto.elements.First(e => e.id == p.element);
                var player = players.First(plr => plr.Id == element.id);
                return element.ToPlayerEvent(player, p);
            })
            .ToList();
    }

    private static PlayerEvent ToPlayerEvent(
        this ElementDTO element,
        PlayerDTO player,
        PickDTO pick
    )
    {
        return new PlayerEvent(
            player.FirstName,
            player.SecondName,
            player.WebName,
            element.id,
            element.stats.minutes,
            element.stats.goals_scored,
            element.stats.assists,
            element.stats.goals_conceded,
            element.stats.clean_sheets,
            element.stats.penalties_saved,
            element.stats.own_goals,
            element.stats.penalties_missed,
            element.stats.yellow_cards,
            element.stats.red_cards,
            element.stats.saves,
            element.stats.bonus,
            element.stats.bps,
            element.stats.influence,
            element.stats.creativity,
            element.stats.threat,
            element.stats.ict_index,
            element.stats.total_points,
            element.stats.in_dreamteam,
            pick.multiplier,
            pick.is_captain,
            pick.is_vice_captain,
            pick.position,
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
