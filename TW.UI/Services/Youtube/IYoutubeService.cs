using TW.UI.Models;

namespace TW.UI.Services
{
    public interface IYoutubeService
    {
        Task<bool> RefreshAccessToken();
        Task<List<PlaylistDisplayGroup>> GetYoutubePlaylists();
        Uri GetAuthorizationLink();
        Task GetAuthorizationToken(string authorizationCode);
    }
}
