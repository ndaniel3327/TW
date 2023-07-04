using TW.UI.Models;
using TW.UI.Models.Spotify.View;

namespace TW.UI.Services.Spotify
{
    public interface ISpotifyService
    {
        Task<Uri> GetAuthorizationUri();
        Task ExchangeCodeForToken(string code);
        Task<List<PlaylistDisplayGroup>> GetPlaylists();
        Task<bool> RefreshAccessToken();
    }
}
