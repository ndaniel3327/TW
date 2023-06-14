using System.Collections.Generic;

namespace TW.UI.Models.Spotify.Data
{
    public class SpotifyPlaylist
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<SpotifyTrack> Tracks { get; set; }
    }
}
