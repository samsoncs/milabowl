using System;
using System.Collections.Generic;
using Milabowl.Domain.Import;
using Milabowl.Domain.Import.FantasyDTOs;
using Moq;

namespace Milabowl.Test.Helpers
{
    public class ImportTest
    {
        public Mock<IDataImportProvider> GetDataProviderMock(int eventId, int teamId, int playerId, int leagueId, string username)
        {
            var dataImportProviderMock = new Mock<IDataImportProvider>();

            dataImportProviderMock.Setup(m => m.GetBootstrapRoot()).ReturnsAsync(this.GetMockedBootstrap(eventId, teamId, playerId));
            dataImportProviderMock.Setup(m => m.GetLeagueRoot()).ReturnsAsync(this.GetMockedLeagueRoot(leagueId, username));
            dataImportProviderMock.Setup(m => m.GetEventRoot(It.IsAny<int>())).ReturnsAsync(this.GetMockedEventRoot(playerId));
            dataImportProviderMock.Setup(m => m.GetPicksRoot(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(this.GetMockedPicksRoot(playerId));

            return dataImportProviderMock;
        }

        private BootstrapRootDTO GetMockedBootstrap(int eventId, int teamId, int playerId)
        {
            return new BootstrapRootDTO
            {
                Events = new List<EventDTO>
                {
                    new EventDTO
                    {
                        DataChecked = true,
                        Finished = true,
                        Id = eventId,
                        DeadlineTime = DateTime.Today.AddDays(-1),
                        Name = "Gameweek 1",
                    }
                },
                Teams = new List<TeamDTO>
                {
                    new TeamDTO
                    {
                        Name = "Arsenal Test",
                        Id = teamId
                    }
                },
                Players = new List<PlayerDTO>
                {
                    new PlayerDTO
                    {
                        FirstName = "Mo",
                        SecondName = "Salah",
                        Id = playerId,
                        Team = teamId,
                    }
                },
                TotalPlayers = 3
            };
        }

        private LeagueRootDTO GetMockedLeagueRoot(int leagueId, string userName)
        {
            return new LeagueRootDTO
            {
                league = new LeagueDTO
                {
                    name = "Super League",
                    id = leagueId,
                },
                new_entries = new NewEntriesDTO(),
                standings = new StandingsDTO
                {
                    results = new List<ResultDTO>
                    {
                        new ResultDTO{ entry = 1, event_total = 1, player_name = userName}
                    }
                }
            };
        }

        private EventRootDTO GetMockedEventRoot(int playerId)
        {
            return new EventRootDTO
            {
                elements = new List<ElementDTO>
                {
                    new ElementDTO
                    {
                        stats = new StatsDTO
                        {
                            total_points = 2,
                        },
                        id = playerId,
                    }
                }
            };
        }

        private PicksRootDTO GetMockedPicksRoot(int playerId)
        {
            return new PicksRootDTO
            {
                picks = new List<PickDTO>
                {
                    new PickDTO
                    {
                        element = playerId,
                        multiplier = 1,
                    }
                }
            };
        }
    }
}
