using TW.UI.Models;

namespace TW.UI.Services
{
    public interface ISpotifyService
    {
        Task<Uri> GetAuthorizationUri();
        Task ExchangeCodeForToken(string code);
        Task<List<PlaylistDisplayGroupModel>> GetPlaylists();
        Task<bool> RefreshAccessToken();
    }
}
