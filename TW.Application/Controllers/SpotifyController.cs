using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TW.Application.Models;
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
        public async Task<ActionResult<Uri>> Get()
        {
            Uri loginUri = await _spotifyService.AuthorizeWithPKCE();

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

            return Ok(_spotifyService.IsLoggedIn());
        }

    }
}
