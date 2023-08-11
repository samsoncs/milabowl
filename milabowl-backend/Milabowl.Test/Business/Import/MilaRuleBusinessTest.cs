using System;
using System.Collections.Generic;
using FluentAssertions;
using Milabowl.Domain.Entities.Fantasy;
using Milabowl.Domain.Processing;
using NUnit.Framework;

namespace Milabowl.Test.Business.Import
{
    [TestFixture]
    public class MilaRuleBusinessTest
    {
        private readonly IMilaRuleBusiness _milaRuleBusiness;

        public MilaRuleBusinessTest()
        {
            this._milaRuleBusiness = new MilaRuleBusiness();
        }

        [Test]
        public void ShouldGetSixPointNineIfTotalPointsIsSixtyNine()
        {
            var milaRuleDatas = new List<MilaRuleData>
            {
                new MilaRuleData{ TotalPoints = 67, Multiplier = 1 }, 
                new MilaRuleData{ TotalPoints = 1, Multiplier = 2 }
            };

            var sixtyNine = this._milaRuleBusiness.GetSixtyNine(milaRuleDatas);

            sixtyNine.Should().Be(6.9m);
        }

        [Test]
        public void ShouldGetZeroIfTotalPointsNotSixtyNine()
        {
            var milaRuleDatas = new List<MilaRuleData>
            {
                new MilaRuleData{ TotalPoints = 68, Multiplier = 1 },
                new MilaRuleData{ TotalPoints = 1, Multiplier = 2 }
            };

            var sixtyNine = this._milaRuleBusiness.GetSixtyNine(milaRuleDatas);

            sixtyNine.Should().Be(0);
        }

        [Test]
        public void ShouldGetOnePointPerYellowCard()
        {
            var milaRuleDatas = new List<MilaRuleData>
            {
                new MilaRuleData{ YellowCards = 1, Multiplier = 1 },
                new MilaRuleData{ YellowCards = 1, Multiplier = 1 }
            };

            var yellowCards = this._milaRuleBusiness.GetYellowCardScore(milaRuleDatas);

            yellowCards.Should().Be(2);
        }

        [Test]
        public void ShouldNotGetPointForYellowCardIfNotOnTeam()
        {
            var milaRuleDatas = new List<MilaRuleData>
            {
                new MilaRuleData{ YellowCards = 1, Multiplier = 0 },
            };

            var yellowCards = this._milaRuleBusiness.GetYellowCardScore(milaRuleDatas);

            yellowCards.Should().Be(0);
        }

        [Test]
        public void ShouldGetPointIfKeepCapped()
        {
            var milaRuleDatas = new List<MilaRuleData>
            {
                new MilaRuleData{ PlayerPosition = 1, Multiplier = 2 },
            };

            var capKeepScore = this._milaRuleBusiness.GetCapKeepScore(milaRuleDatas);

            capKeepScore.Should().Be(2);
        }

        [Test]
        public void ShouldNotGetCapKeepPointsIfKeepNotCapped()
        {
            var milaRuleDatas = new List<MilaRuleData>
            {
                new MilaRuleData{ PlayerPosition = 1, Multiplier = 1 },
                new MilaRuleData{ PlayerPosition = 1, Multiplier = 0 },
            };

            var capKeepScore = this._milaRuleBusiness.GetCapKeepScore(milaRuleDatas);

            capKeepScore.Should().Be(0);
        }

        [Test]
        public void ShouldGetPointIfDefCapped()
        {
            var player = new Player { PlayerId = Guid.NewGuid() };
            
            var milaRuleDatas = new List<MilaRuleData>
            {
                new MilaRuleData{ PlayerPosition = 2, Multiplier = 2, Player = player },
            };

            var capKeepScore = this._milaRuleBusiness.GetCapDefScore(milaRuleDatas, player);

            capKeepScore.Should().Be(1);
        }

        [Test]
        public void ShouldNotGetCapDefPointsIfDefNotCapped()
        {
            var player = new Player { PlayerId = Guid.NewGuid() };

            var milaRuleDatas = new List<MilaRuleData>
            {
                new MilaRuleData{ PlayerPosition = 2, Multiplier = 1, Player = player },
                new MilaRuleData{ PlayerPosition = 2, Multiplier = 0, Player = player  },
            };

            var capKeepScore = this._milaRuleBusiness.GetCapDefScore(milaRuleDatas, player);

            capKeepScore.Should().Be(0);
        }

        [Test]
        public void ShouldGetUniqueCaptainScoreIfUniqueCaptain()
        {
            var userCaptain = new Player{ PlayerId = Guid.NewGuid() };

            var lineups = new List<Lineup>
            {
                GetLineup(userCaptain),
                GetLineup()
            };

            var uniqueCaptainScore = this._milaRuleBusiness.GetUniqueCaptainScore(userCaptain, lineups);
            uniqueCaptainScore.Should().Be(2);
        }

        [Test]
        public void ShouldGetZeroIfCaptainNotUnique()
        {
            var userCaptain = new Player { PlayerId = Guid.NewGuid() };

            var lineups = new List<Lineup>
            {
                GetLineup(userCaptain),
                GetLineup(userCaptain),
                GetLineup()
            };

            var uniqueCaptainScore = this._milaRuleBusiness.GetUniqueCaptainScore(userCaptain, lineups);
            uniqueCaptainScore.Should().Be(0);
        }

        [Test]
        public void ShouldGetSixtyNineSubScoreIfAnyPlayersWith69Mins()
        {
            var milaRuleDatas = new List<MilaRuleData>
            {
                new (){ Minutes = 60, IsCaptain = true },
                new (){ Minutes = 69, IsCaptain = false, },
            };

            var sixtyNineSub = this._milaRuleBusiness.GetSixtyNineSub(milaRuleDatas);

            sixtyNineSub.Should().Be(2.69m);
        }

        [Test]
        public void ShouldGetSixtyNineSubScoreMultipliedIfCapWith69Mins()
        {
            var milaRuleDatas = new List<MilaRuleData>
            {
                new (){ Minutes = 60, IsCaptain = false },
                new (){ Minutes = 69, IsCaptain = true, Multiplier = 2 },
            };

            var sixtyNineSub = this._milaRuleBusiness.GetSixtyNineSub(milaRuleDatas);

            sixtyNineSub.Should().Be(2.69m * 2);
        }

        [Test]
        public void ShouldNotGetSixtyNineSubScoreIfNoPlayersWith69Mins()
        {
            var milaRuleDatas = new List<MilaRuleData>
            {
                new (){ Minutes = 68, IsCaptain = true },
                new (){ Minutes = 70, IsCaptain = false, },
            };

            var sixtyNineSub = this._milaRuleBusiness.GetSixtyNineSub(milaRuleDatas);

            sixtyNineSub.Should().Be(0);
        }

        [TestCase(true, 45, 44, 2)]
        [TestCase(true, 45, 43, 2)]
        [TestCase(true, 45, 42, 0)]
        [TestCase(false, 45, 45, 0)]
        [TestCase(false, 45, 46, 0)]
        [TestCase(true, 45, 46, 0)]

        public void ShouldGetHeaDataHeadMetaScoreIfWinWithLessThan2(bool didWin, int myPoints, int opponentPoints, int expected)
        {
            var userHeaDataHead = new UserHeadToHead
            {
                DidWin = didWin,
                UserPoints = myPoints,
                OpponentPoints = opponentPoints
            };

            var sixtyNineSub = this._milaRuleBusiness.GetHeadToHeadMetaScore(userHeaDataHead);

            sixtyNineSub.Should().Be(expected);
        }

        public void ShouldGetHeaDataHeadScore()
        {

        }

        private Lineup GetLineup(Player captain = null)
        {
            return new()
            {
                PlayerEventLineups = new List<PlayerEventLineup>
                {
                    new()
                    {
                        IsCaptain = true,
                        PlayerEvent = new PlayerEvent
                        {
                            Player = captain ?? new Player(),
                            FkPlayerId = captain?.PlayerId ?? Guid.NewGuid()
                        }
                    },
                    new()
                    {
                        IsCaptain = false,
                        PlayerEvent = new PlayerEvent
                        {
                            Player = new Player()
                        }
                    }
                }
            };
        }

    }
}
