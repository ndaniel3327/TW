using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TW.Infrastracture.Services.Spotify;
using TW.Infrastructure.Models;

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
            var result = await _spotifyService.GetCallback(code);

            var response = new { AccessToken = result.AccessToken, ExpiresInSeconds = result.ExpiresIn, RefreshToken = result.RefreshToken, CreatedAt = result.CreatedAt , TokenType=result.TokenType};

            return Ok(response);
        }

        [HttpGet("playlists")]
        public async Task<ActionResult<List<Playlist>>> GetPlaylists()
        {
            var playlists = await _spotifyService.GetPlaylists();

            return Ok(playlists);
        }

        [HttpPost("refreshToken")]
        public async Task<ActionResult<int>> RefrehToken([FromBody]string refreshToken)
        {
            var result = await _spotifyService.RefreshAccessToken(refreshToken);

            var response = new {ExpiresInSeconds=result.ExpiresIn,CreatedAt=result.CreatedAt};

            return Ok(response);
        }
        // TODO:(partially) Add Middleware to forward exceptions
    }
}
