using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Get()
        {
            _spotifyService.AuthorizeAndGetToken();
            return Ok();
        }
    }
}
