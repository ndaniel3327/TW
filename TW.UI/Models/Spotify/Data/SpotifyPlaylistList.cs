using System.Text.Json.Serialization;

namespace TW.UI.Models.Spotify.Data
{
    public class SpotifyPlaylistList
    {
        [JsonPropertyName("items")]
        public List<SpotifyPlaylist> Playlists { get; set; }
    }
}
