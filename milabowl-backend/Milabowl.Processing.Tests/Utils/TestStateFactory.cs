using Bogus;
using Milabowl.Processing.DataImport.FplDtos;
using Milabowl.Processing.DataImport.Models;
using Milabowl.Processing.Processing;

namespace Milabowl.Processing.Tests.Utils;

public class MilaGameWeekStateBuilder
{
    private readonly Faker _faker = new();
    private UserState _user;
    private IList<UserState> _opponents;

    public MilaGameWeekStateBuilder()
    {
        _user = new UserStateBuilder().Build();
        _opponents = new List<UserState>
        {
            new UserStateBuilder().Build(),
            new UserStateBuilder().Build()
        };
    }

    public MilaGameWeekStateBuilder WithOpponents(params IList<UserState> opponents)
    {
        _opponents = opponents;
        return this;
    }

    public MilaGameWeekStateBuilder WithUser(UserState user)
    {
        _user = user;
        return this;
    }

    public MilaGameWeekStateBuilder WithLineup(params IList<PlayerEvent> lineup)
    {
        _user = _user with
        {
            Lineup = lineup.AsReadOnly()
        };
        return this;
    }

    public MilaGameWeekStateBuilder WithSubsIn(params IList<Sub> subs)
    {
        _user = _user with
        {
            SubsIn = subs.AsReadOnly()
        };
        return this;
    }

    public MilaGameWeekStateBuilder WithSubsOut(params IList<Sub> subs)
    {
        _user = _user with
        {
            SubsOut = subs.AsReadOnly()
        };
        return this;
    }

    public MilaGameWeekStateBuilder WithHeadToHead(HeadToHead headToHead)
    {
        _user = _user with
        {
            HeadToHead = headToHead
        };
        return this;
    }

    public MilaGameWeekState Build()
    {
        return new MilaGameWeekState
        {
            User = _user,
            Opponents = _opponents.AsReadOnly()
        };
    }
}

public class UserStateBuilder
    {
        private readonly Faker _faker = new();
        private Event _event;
        private HeadToHead _headToHead;
        private int _totalScore;
        private IList<PlayerEvent> _lineup;
        private string _activeChip;
        private IList<UserState> _history;
        private IList<Sub> _subsIn;
        private IList<Sub> _subsOut;
        private EventRootDTO _eventRoot;

        public UserStateBuilder()
        {
            _event = new Event(_faker.Random.Int(0, 38), _faker.Lorem.Word());
            _headToHead = TestStateFactory.GetHeadToHead(_faker.Random.Int(10), _faker.Random.Int(10)).Generate();
            _totalScore = _faker.Random.Int(0, 200);
            _lineup = TestStateFactory.GetPlayerEvent().Generate(15);
            _activeChip = "freehit";
            _history = new List<UserState>();
            _eventRoot = new EventRootDTO();
            _subsIn = TestStateFactory.GetSub().Generate(1);
            _subsOut = TestStateFactory.GetSub().Generate(1);
        }

        public UserStateBuilder WithEvent(Event @event)
        {
            _event = @event;
            return this;
        }

        public UserStateBuilder WithHeadToHead(HeadToHead headToHead)
        {
            _headToHead = headToHead;
            return this;
        }

        public UserStateBuilder WithTotalScore(int totalScore)
        {
            _totalScore = totalScore;
            return this;
        }

        public UserStateBuilder WithLineup(params IList<PlayerEvent> lineup)
        {
            _lineup = lineup;
            return this;
        }

        public UserStateBuilder WithSubsIn(params IList<Sub> subsIn)
        {
            _subsIn = subsIn;
            return this;
        }

        public UserStateBuilder WithSubsOut(params IList<Sub> subsOut)
        {
            _subsOut = subsOut;
            return this;
        }

        public UserStateBuilder WithActiveChip(string activeChip)
        {
            _activeChip = activeChip;
            return this;
        }

        public UserStateBuilder WithHistory(IList<UserState> history)
        {
            _history = history;
            return this;
        }

        public UserStateBuilder WithEventRoot(EventRootDTO eventRoot)
        {
            _eventRoot = eventRoot;
            return this;
        }

        public UserState Build()
        {
            return new UserState
            {
                ActiveChip = _activeChip,
                Event = _event,
                HeadToHead = _headToHead,
                History = _history,
                Lineup = _lineup.AsReadOnly(),
                TotalScore = _totalScore,
                User = new User(
                    _faker.Random.Int(0, 100000),
                    _faker.Random.Int(0, 100000),
                    _faker.Name.FullName(),
                    _faker.Company.CompanyName(),
                    _faker.Random.Int(0, 100000),
                    _faker.Random.Int(0, 100000),
                    _faker.Random.Int(0, 200)
                ),
                SubsIn = _subsIn.AsReadOnly(),
                SubsOut = _subsOut.AsReadOnly(),
            };
        }
    }

public static class TestStateFactory
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

    public static Faker<Sub> GetSub()
    {
        return new Faker<Sub>()
            .RuleFor(r => r.FantasyPlayerEventId, f => f.Random.Int(1, 100000))
            .RuleFor(r => r.FirstName, f => f.Name.FirstName())
            .RuleFor(r => r.Surname, f => f.Name.LastName())
            .RuleFor(r => r.TotalPoints, f => f.Random.Int(0, 10));
    }

    public static Faker<PlayerEvent> GetPlayerEvent()
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

    public static Faker<HeadToHead> GetHeadToHead(int currentUserScore, int opponentScore)
    {
        return new Faker<HeadToHead>()
            .CustomInstantiator(f => new HeadToHead(
                GetHeadToHeadEvent().RuleFor(r => r.Points, currentUserScore).Generate(),
                GetHeadToHeadEvent().RuleFor(r => r.Points, opponentScore).Generate()
            ));
    }

    public static Faker<HeadToHeadEvent> GetHeadToHeadEvent()
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
}
