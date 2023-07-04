using System.Text.Json.Serialization;

namespace TW.UI.Models.Spotify.View
{
    public class SpotifyTrackView
    {
        public List<string> ArtistsNames { get; set; }

        public string Artists => string.Join(" and ", ArtistsNames);

        public string Name { get; set; }
    }
}