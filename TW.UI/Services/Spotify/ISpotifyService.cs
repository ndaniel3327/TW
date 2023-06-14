using TW.UI.Models.Spotify.Data;

namespace TW.UI.Services.Spotify
{
    public interface ISpotifyService
    {
        Task<Uri> StartAuthorizationWithPKCE();
        Task ExchangeCodeForToken(string code);
        Task<List<SpotifyPlaylist>> GetPlaylists();
        Task<bool> RefreshAccessToken(string refreshToken);
    }
}
