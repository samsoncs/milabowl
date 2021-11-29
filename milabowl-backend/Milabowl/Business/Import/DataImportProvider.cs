using System.Net.Http;
using System.Threading.Tasks;
using Milabowl.Business.DTOs.Import;
using Milabowl.Utils;

namespace Milabowl.Business.Import
{
    public interface IDataImportProvider
    {
        Task<BootstrapRootDTO> GetBootstrapRoot();
        Task<LeagueRootDTO> GetLeagueRoot();
        Task<EventRootDTO> GetEventRoot(int eventID);
        Task<HeadToHeadEventRootDTO> GetHead2HeadEventRoot(int eventID);
        Task<PicksRootDTO> GetPicksRoot(int eventID, int userID);
    }

    public class DataImportProvider: IDataImportProvider
    {
        private readonly HttpClient _httpClient;

        public DataImportProvider(IHttpClientFactory httpClientFactory)
        {
            this._httpClient = httpClientFactory.CreateClient();
        }

        public async Task<BootstrapRootDTO> GetBootstrapRoot()
        {
            return await this._httpClient.GetDeserializedAsync<BootstrapRootDTO>("https://fantasy.premierleague.com/api/bootstrap-static/");
        }

        public async Task<LeagueRootDTO> GetLeagueRoot()
        {
            return await this._httpClient.GetDeserializedAsync<LeagueRootDTO>("https://fantasy.premierleague.com/api/leagues-classic/1342357/standings/?page_new_entries=1&page_standings=1&phase=1");
        }

        public async Task<EventRootDTO> GetEventRoot(int eventID)
        {
            return await this._httpClient.GetDeserializedAsync<EventRootDTO>($"https://fantasy.premierleague.com/api/event/{eventID}/live/");
        }

        public async Task<PicksRootDTO> GetPicksRoot(int eventID, int userID)
        {
            return await this._httpClient.GetDeserializedAsync<PicksRootDTO>($@"https://fantasy.premierleague.com/api/entry/{userID}/event/{eventID}/picks/");
        }

        public async Task<HeadToHeadEventRootDTO> GetHead2HeadEventRoot(int eventID)
        {
            return await this._httpClient.GetDeserializedAsync<HeadToHeadEventRootDTO>($"https://fantasy.premierleague.com/api/leagues-h2h-matches/league/1345045/?page=1&event={eventID}");
        }
    }
}
