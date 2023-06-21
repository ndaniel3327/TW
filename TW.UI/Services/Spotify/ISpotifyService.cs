using TW.UI.Models.Spotify.View;

namespace TW.UI.Services.Spotify
{
    public interface ISpotifyService
    {
        Task<Uri> GetAuthorizationUri();
        Task ExchangeCodeForToken(string code);
        Task<List<SpotifyPlaylistGroup>> GetPlaylists();
        Task<bool> RefreshAccessToken();
    }
}
