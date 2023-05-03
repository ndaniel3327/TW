using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using TW.Application.Services;

namespace TW.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpotifyController : ControllerBase
    {
        private readonly ISpotifyService _spotifyService;

        public SpotifyController(ISpotifyService spotifyService)
        {
            _spotifyService = spotifyService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            Debug.WriteLine("Application received a GET Request at /api/spotify");
            await _spotifyService.AuthorizeAndGetTokenOld();

            return Ok();
        }
    }
}
