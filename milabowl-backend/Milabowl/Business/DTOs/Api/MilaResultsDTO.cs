using System.Collections.Generic;

namespace Milabowl.Business.DTOs.Api
{
    public class MilaResultsDTO
    {
        public IList<GameWeekDTO> ResultsByWeek { get; set; }
        public IList<UserResultsDTO> ResultsByUser { get; set; }
        public IList<MilaResultDTO> OverallScore { get; set; }
    }

    public class GameWeekDTO
    {
        public IList<MilaResultDTO> Results { get; set; }
        public int GameWeek { get; set; }
    }

    public class UserResultsDTO
    {
        public IList<MilaResultDTO> Results { get; set; }
        public string TeamName { get; set; }
    }
}
