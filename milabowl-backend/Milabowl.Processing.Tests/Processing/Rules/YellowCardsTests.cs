using Milabowl.Processing.Processing.Rules;
using Milabowl.Processing.Tests.Utils;
using Shouldly;

namespace Milabowl.Processing.Tests.Processing.Rules;

public class YellowCardsTests : MilaRuleTest<YellowCards>
{
    [Fact]
    public void Should_get_one_point_for_every_playing_player()
    {
        var state = new ManagerGameWeekStateBuilder()
            .WithLineup(
                TestStateFactory.GetPlayer().RuleFor(r => r.YellowCards, 1),
                TestStateFactory.GetCaptain().RuleFor(r => r.YellowCards, 1),
                TestStateFactory.GetPlayer().RuleFor(r => r.YellowCards, 0),
                TestStateFactory.GetBenchPlayer().RuleFor(r => r.YellowCards, 1)
            )
            .Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(3);
    }
}
