using Bogus;
using Milabowl.Processing.DataImport.FplDtos;
using Milabowl.Processing.DataImport.Models;
using Milabowl.Processing.Processing;

namespace Milabowl.Processing.Tests.Utils;

public static class StateFactory
{
    public static Faker<PlayerEvent> GetPlayer()
    {
        return GetPlayerEvent().RuleFor(r => r.Multiplier, 1);
    }

    public static Faker<PlayerEvent> GetCaptain()
    {
        return GetPlayerEvent().RuleFor(r => r.Multiplier, 2);
    }

    public static Faker<PlayerEvent> GetBenchPlayer()
    {
        return GetPlayerEvent().RuleFor(r => r.Multiplier, 0);
    }

    private static Faker<PlayerEvent> GetPlayerEvent()
    {

        return new Faker<PlayerEvent>()
            .CustomInstantiator(f =>
                new PlayerEvent(
                    "",
                    "",
                    "",
                    1,
                    1,
                    1,
                    "",
                    "",
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    "",
                    "",
                    "",
                    "",
                    8,
                    true,
                    1,
                    true,
                    true,
                    PlayerPosition.MID,
                    "Mid"
                )
            );
    }

    public static Faker<HeadToHead> GetHeadToHead()
    {
        return new Faker<HeadToHead>()
            .CustomInstantiator(f => new HeadToHead(
                GetHeadToHeadEvent().Generate(),
                GetHeadToHeadEvent().Generate()
            ));
    }

    public static Faker<MilaGameWeekState> GetMilaGameWeekState(
        IList<PlayerEvent>? playerEvents = null,
        HeadToHead? userHeadToHead = null,
        IList<UserState>? opponents = null,
        int? userTotalScore = null)
    {
        var lineup = playerEvents ?? GetPlayerEvent().Generate(15);
        var headToHead = userHeadToHead ?? GetHeadToHead().Generate();
        var userState = new Faker<UserState>()
            .CustomInstantiator(f => new UserState(
                    new Event(f.Random.Int(0, 38), f.Lorem.Word()),
                    headToHead,
                    new User(
                        f.Random.Int(0, 100000),
                        f.Random.Int(0, 100000),
                        f.Name.FullName(),
                        f.Company.CompanyName(),
                        f.Random.Int(0, 100000),
                        f.Random.Int(0, 100000),
                        f.Random.Int(0, 200)
                    ),
                    lineup,
                    "freehit",
                    new List<UserState>(),
                    new EventRootDTO()
                )
            ).RuleFor(r => r.Lineup, GetPlayerEvent().Generate(11))
            .RuleFor(r => r.TotalScore, userTotalScore ?? 0);

        return new Faker<MilaGameWeekState>()
            .CustomInstantiator(f =>
                new MilaGameWeekState(
                    userState,
                    opponents ?? new List<UserState>()));
    }

    private static Faker<HeadToHeadEvent> GetHeadToHeadEvent()
    {
        return new Faker<HeadToHeadEvent>()
            .CustomInstantiator(f =>
                new HeadToHeadEvent(
                    f.Random.Int(0, 200),
                    f.Random.Bool(),
                    f.Random.Bool(),
                    f.Random.Bool(),
                    f.Random.Int(0, 200),
                    f.Random.Bool(),
                    f.Random.Int(0, 60000),
                    f.Random.Bool(),
                    f.Random.Int(0, 100000)
                )
            );
    }

    public static Faker<BombHolder> GetBombHolder()
    {
        return new Faker<BombHolder>()
            .CustomInstantiator(f => new BombHolder(
                f.Random.Int(1, 100000),
                f.Company.CompanyName(),
                f.Name.FullName()
            ));
    }

    public static Faker<ManagerBombState> GetManagerBombState()
    {
        return new Faker<ManagerBombState>()
            .CustomInstantiator(f => new ManagerBombState(
                f.PickRandom<BombStateEnum>(),
                GetBombHolder().Generate(),
                f.Random.Bool() ? GetBombHolder().Generate() : null
            ));
    }

    public static List<Sub> GetSubs(params (string firstName, string surname, int totalPoints)[] subs)
    {
        return subs.Select(s => new Sub(s.totalPoints, s.firstName, s.surname, s.totalPoints)).ToList();
    }
}
