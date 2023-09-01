using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TW.UI.Models;

namespace TW.UI.Services
{
    public interface IDisplayedPlaylistsService
    {
        List<PlaylistDisplayGroup> UpdateDisplayedSpotifyPlaylists(List<PlaylistDisplayGroup> allPlaylists);
        List<PlaylistDisplayGroup> UpdateDisplayedYoutbePlaylists(List<PlaylistDisplayGroup> allPlaylists);
        List<PlaylistDisplayGroup> UpdateDisplayedLocalPlaylists(List<PlaylistDisplayGroup> allPlaylists);
    }
}
