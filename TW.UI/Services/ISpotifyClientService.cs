
using TW.UI.Models;

namespace TW.UI.Services
{
    public interface ISpotifyClientService

    {
        Task<Uri> AuthorizeSpotify();
        Task<List<SpotifyPlaylistModel>> GetPlaylists();
        Task<bool> IsLoggedIn();
    }
}


