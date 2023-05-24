using TW.UI.Models;

namespace TW.UI.Services.Spotify
{
    public interface ISpotifyClientService

    {
        Task<Uri> AuthorizeSpotify();
        Task<List<SpotifyPlaylistModel>> GetPlaylists();
        Task<bool> IsLoggedIn();
    }
}


