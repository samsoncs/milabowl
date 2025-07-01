using Milabowl.Processing.Processing.Rules;
using Milabowl.Processing.Tests.Utils;
using Shouldly;

namespace Milabowl.Processing.Tests.Processing.Rules;

public class TrendyBitchTests : MilaRuleTest<TrendyBitch>
{
    [Fact]
    public void Should_get_minus_point_if_player_traded_in_most_traded_in_player()
    {
        var mostTradedIn = TestStateFactory.GetSub().Generate();
        var leastTradedIn = mostTradedIn with
        {
            FantasyPlayerEventId = mostTradedIn.FantasyPlayerEventId - 1,
        };
        var leastTradedIn2 = mostTradedIn with
        {
            FantasyPlayerEventId = mostTradedIn.FantasyPlayerEventId + 1,
        };
        var opponent = new ManagerGameWeekStateBuilder()
            .WithSubsIn(mostTradedIn, leastTradedIn)
            .Build();
        var state = new ManagerGameWeekStateBuilder()
            .WithLineup()
            .WithSubsIn(mostTradedIn, leastTradedIn2)
            .WithOpponents(opponent)
            .Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(-1);
    }

    [Fact]
    public void Should_get_minus_point_if_player_traded_out_most_traded_out_player()
    {
        var mostTradedOut = TestStateFactory.GetSub().Generate();
        var leastTradedOut = mostTradedOut with
        {
            FantasyPlayerEventId = mostTradedOut.FantasyPlayerEventId - 1,
        };
        var leastTradedOut2 = mostTradedOut with
        {
            FantasyPlayerEventId = mostTradedOut.FantasyPlayerEventId + 1,
        };
        var opponent = new ManagerGameWeekStateBuilder()
            .WithSubsOut(mostTradedOut, leastTradedOut)
            .Build();
        var state = new ManagerGameWeekStateBuilder()
            .WithLineup()
            .WithSubsOut(mostTradedOut, leastTradedOut2)
            .WithOpponents(opponent)
            .Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(-1);
    }

    [Fact]
    public void Should_get_minus_2_points_if_player_traded_out_and_in_most_traded_players()
    {
        var mostTradedOut = TestStateFactory.GetSub().Generate();
        var mostTradedIn = TestStateFactory.GetSub().Generate();
        var opponent = new ManagerGameWeekStateBuilder()
            .WithSubsIn(mostTradedIn)
            .WithSubsOut(mostTradedOut)
            .Build();
        var state = new ManagerGameWeekStateBuilder()
            .WithLineup()
            .WithSubsIn(mostTradedIn)
            .WithSubsOut(mostTradedOut)
            .WithOpponents(opponent)
            .Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(-2);
    }

    [Fact]
    public void Should_get_zero_points_if_player_did_not_trade_most_popular_trades()
    {
        var mostTradedOut = TestStateFactory.GetSub().Generate();
        var leastTradedOut = mostTradedOut with
        {
            FantasyPlayerEventId = mostTradedOut.FantasyPlayerEventId - 1,
        };
        var mostTradedIn = TestStateFactory.GetSub().Generate();
        var leastTradedIn = mostTradedIn with
        {
            FantasyPlayerEventId = mostTradedIn.FantasyPlayerEventId - 1,
        };
        var opponent1 = new ManagerGameWeekStateBuilder()
            .WithSubsIn(mostTradedIn)
            .WithSubsOut(mostTradedOut)
            .Build();
        var opponent2 = new ManagerGameWeekStateBuilder()
            .WithSubsIn(mostTradedIn)
            .WithSubsOut(mostTradedOut)
            .Build();
        var state = new ManagerGameWeekStateBuilder()
            .WithLineup()
            .WithSubsIn(leastTradedIn)
            .WithSubsOut(leastTradedOut)
            .WithOpponents(opponent1, opponent2)
            .Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(0);
    }
}
