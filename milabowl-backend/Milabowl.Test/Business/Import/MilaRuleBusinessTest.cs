using System.Collections.Generic;
using FluentAssertions;
using Milabowl.Business.DTOs;
using Milabowl.Business.Import;
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
            var milaRuleDTOs = new List<MilaRuleDTO>
            {
                new MilaRuleDTO{ TotalPoints = 67, Multiplier = 1 }, 
                new MilaRuleDTO{ TotalPoints = 1, Multiplier = 2 }
            };

            var sixtyNine = this._milaRuleBusiness.GetSixtyNine(milaRuleDTOs);

            sixtyNine.Should().Be(6.9m);
        }

        [Test]
        public void ShouldGetZeroIfTotalPointsNotSixtyNine()
        {
            var milaRuleDTOs = new List<MilaRuleDTO>
            {
                new MilaRuleDTO{ TotalPoints = 68, Multiplier = 1 },
                new MilaRuleDTO{ TotalPoints = 1, Multiplier = 2 }
            };

            var sixtyNine = this._milaRuleBusiness.GetSixtyNine(milaRuleDTOs);

            sixtyNine.Should().Be(0);
        }

        [Test]
        public void ShouldGetOnePointPerYellowCard()
        {
            var milaRuleDTOs = new List<MilaRuleDTO>
            {
                new MilaRuleDTO{ YellowCards = 1, Multiplier = 1 },
                new MilaRuleDTO{ YellowCards = 1, Multiplier = 1 }
            };

            var yellowCards = this._milaRuleBusiness.GetYellowCardScore(milaRuleDTOs);

            yellowCards.Should().Be(2);
        }

        [Test]
        public void ShouldNotGetPointForYellowCardIfNotOnTeam()
        {
            var milaRuleDTOs = new List<MilaRuleDTO>
            {
                new MilaRuleDTO{ YellowCards = 1, Multiplier = 0 },
            };

            var yellowCards = this._milaRuleBusiness.GetYellowCardScore(milaRuleDTOs);

            yellowCards.Should().Be(0);
        }

        [Test]
        public void ShouldGetPointIfKeepCapped()
        {
            var milaRuleDTOs = new List<MilaRuleDTO>
            {
                new MilaRuleDTO{ PlayerPosition = 1, Multiplier = 2 },
            };

            var capKeepScore = this._milaRuleBusiness.GetCapKeepScore(milaRuleDTOs);

            capKeepScore.Should().Be(2);
        }

        [Test]
        public void ShouldNotGetCapKeepPointsIfKeepNotCapped()
        {
            var milaRuleDTOs = new List<MilaRuleDTO>
            {
                new MilaRuleDTO{ PlayerPosition = 1, Multiplier = 1 },
                new MilaRuleDTO{ PlayerPosition = 1, Multiplier = 0 },
            };

            var capKeepScore = this._milaRuleBusiness.GetCapKeepScore(milaRuleDTOs);

            capKeepScore.Should().Be(0);
        }

        [Test]
        public void ShouldGetPointIfDefCapped()
        {
            var milaRuleDTOs = new List<MilaRuleDTO>
            {
                new MilaRuleDTO{ PlayerPosition = 2, Multiplier = 2 },
            };

            var capKeepScore = this._milaRuleBusiness.GetCapDefScore(milaRuleDTOs);

            capKeepScore.Should().Be(1);
        }

        [Test]
        public void ShouldNotGetCapDefPointsIfDefNotCapped()
        {
            var milaRuleDTOs = new List<MilaRuleDTO>
            {
                new MilaRuleDTO{ PlayerPosition = 2, Multiplier = 1 },
                new MilaRuleDTO{ PlayerPosition = 2, Multiplier = 0 },
            };

            var capKeepScore = this._milaRuleBusiness.GetCapDefScore(milaRuleDTOs);

            capKeepScore.Should().Be(0);
        }

    }
}
