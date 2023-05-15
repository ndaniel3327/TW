using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TW.Infrastructure.Models;
using TW.Infrastructure.Services;

namespace TW.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpotifyController : ControllerBase
    {
        private readonly ISpotifyServerService _spotifyService;

        public SpotifyController(ISpotifyServerService spotifyService)
        {
            _spotifyService = spotifyService;
        }

        [HttpGet]
        public async Task<ActionResult<Uri>> Get()
        {
            var loginUri = await Task.Run(() => _spotifyService.AuthorizeWithPKCE());

            return Ok(loginUri);
        }

        [HttpGet("callback")]
        public async Task<ActionResult> GetCallback([FromQuery]string code)
        {
            await _spotifyService.GetCallback(code);

            return Ok();
        }

        [HttpGet("playlists")]
        public async Task<ActionResult<List<Playlist>>> GetPlaylists()
        {
            var playlists = await _spotifyService.GetPlaylists();

            return Ok(playlists);
        }

        [HttpGet("isloggedin")]
        public async Task<ActionResult<bool>> IsLoggedIn()
        {
            var isLoggedId = await Task.Run(() => _spotifyService.IsLoggedIn); 
            return Ok();
        }

        // TODO:(partially) Add Middleware to forward exceptions


    }
}
