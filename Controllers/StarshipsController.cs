using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarWarsAPIChallenge.Services;
using System.Threading.Tasks;

namespace StarWarsAPIChallenge.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StarshipsController : ControllerBase
    {
        private readonly StarshipService _starshipService;

        public StarshipsController(StarshipService starshipService)
        {
            _starshipService = starshipService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetStarships([FromQuery] string? manufacturer)
        {
            try
            {
                var starships = await _starshipService.GetStarshipsAsync(manufacturer);

                if (starships.Count == 0)
                {
                    return NoContent();
                }

                return Ok(starships);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
