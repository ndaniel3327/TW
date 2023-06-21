using System.Collections.ObjectModel;
using TW.UI.Models.Spotify.Data;

namespace TW.UI.Models.Spotify.View
{
    public class SpotifyPlaylistGroup
        : List<SpotifyTrackView>
    {
        public string Name { get; set; }
        public List<SpotifyTrackView> Tracks { get; set; }
    }
}
