using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Milabowl.Utils
{
    public static class HttpClientExtensions
    {
        public static async Task<T> GetDeserializedAsync<T>(this HttpClient httpClient, string url)
        {
            var response = await httpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

        }
    }
}
