using System.Text.Json.Serialization;

namespace TW.UI.Models.Spotify.Data
{
    public class SpotifyTrackList
    {
        [JsonPropertyName("items")]
        public List<SpotifyTrack> Tracks { get; set; }
    }
}
