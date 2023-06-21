using System.Text.Json.Serialization;

namespace TW.UI.Models.Spotify.Data
{
    public class SpotifyArtist
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
