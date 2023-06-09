using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TW.Infrastructure.Models;

namespace TW.Infrastracture.Services.Spotify
{
    public interface ISpotifyServerService
    {
        Task<Uri> AuthorizeWithPKCE();
        Task<PKCETokenResponse> GetCallback(string code);
        Task<List<Playlist>> GetPlaylists();
        Task<PKCETokenResponse> RefreshAccessToken(string refreshToken);
    }
}
