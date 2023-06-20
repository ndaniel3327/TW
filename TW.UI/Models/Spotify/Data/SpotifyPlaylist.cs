using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TW.UI.Models.Spotify.Data
{
    public class SpotifyPlaylist
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
        public List<SpotifyTrack> Tracks { get; set; }
    }
}
