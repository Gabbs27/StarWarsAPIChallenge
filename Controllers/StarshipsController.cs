using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class StarshipsController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public StarshipsController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetStarships([FromQuery] string manufacturer)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync("https://swapi.dev/api/starships/");

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, "Error fetching starships");
        }

        var starshipsData = await response.Content.ReadAsStringAsync();
        var starships = JsonSerializer.Deserialize<StarshipsResponse>(starshipsData);


        if (!string.IsNullOrEmpty(manufacturer))
        {
            starships.Results = starships.Results
                .Where(s => s.Manufacturer.Contains(manufacturer, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return Ok(starships.Results);
    }
}

public class Starship
{
    public string Name { get; set; }
    public string Manufacturer { get; set; }
}

public class StarshipsResponse
{
    public List<Starship> Results { get; set; }
}
