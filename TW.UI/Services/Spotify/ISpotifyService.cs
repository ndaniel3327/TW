using TW.UI.Models;

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
