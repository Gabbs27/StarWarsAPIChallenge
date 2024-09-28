using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using StarWarsAPIChallenge.Models;
using System.Linq;

namespace StarWarsAPIChallenge.Services
{
    public class StarshipService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public StarshipService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<Starship>> GetStarshipsAsync(string? manufacturer)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://swapi.dev/api/starships/");

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Error fetching starships");
            }

            var starshipsData = await response.Content.ReadAsStringAsync();
            var starshipsResponse = JsonSerializer.Deserialize<StarshipsResponse>(starshipsData);

            if (starshipsResponse?.Results == null || starshipsResponse.Results.Count == 0)
            {
                return new List<Starship>();
            }

            // Filtrar por fabricante si se proporciona
            if (!string.IsNullOrEmpty(manufacturer))
            {
                starshipsResponse.Results = starshipsResponse.Results
                    .Where(s => s.Manufacturer.Contains(manufacturer, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return starshipsResponse.Results;
        }
    }
}
