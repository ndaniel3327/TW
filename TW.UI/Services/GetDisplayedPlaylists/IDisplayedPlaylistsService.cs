using TW.UI.Models;

namespace TW.UI.Services
{
    public interface IDisplayedPlaylistsService
    {
        List<PlaylistDisplayGroupModel> UpdateDisplayedSpotifyPlaylists(List<PlaylistDisplayGroupModel> allPlaylists);
        List<PlaylistDisplayGroupModel> UpdateDisplayedYoutbePlaylists(List<PlaylistDisplayGroupModel> allPlaylists);
        List<PlaylistDisplayGroupModel> UpdateDisplayedLocalPlaylists(List<PlaylistDisplayGroupModel> allPlaylists);
    }
}
