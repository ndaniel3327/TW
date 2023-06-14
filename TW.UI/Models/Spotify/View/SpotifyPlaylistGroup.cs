using System.Collections.ObjectModel;
using TW.UI.Models.Spotify.Data;

namespace TW.UI.Models.Spotify.View
{
    public class SpotifyPlaylistGroup : List<SpotifyTrack>
    {
        public string Name { get; set; }
        public SpotifyPlaylistGroup(string name, List<SpotifyTrack> tracks) : base(tracks)
        {
            Name = name;
        }
    }
}
