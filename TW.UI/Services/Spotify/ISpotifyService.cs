using TW.UI.Models.Spotify.Data;

namespace TW.UI.Services.Spotify
{
    public interface ISpotifyService
    {
        Task<Uri> GetAuthorizationUri();
        Task ExchangeCodeForToken(string code);
        Task<List<SpotifyPlaylist>> GetPlaylists();
        Task<bool> RefreshAccessToken();
    }
}
