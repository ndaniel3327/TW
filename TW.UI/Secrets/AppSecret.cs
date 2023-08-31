using System.Text.Json.Serialization;

namespace TW.UI.Secrets
{
    public class AppSecret
    {
        [JsonPropertyName("spotifyid")]
        public string SpotifyId { get; set; }

        [JsonPropertyName("youtubeid")]
        public string YoutubeId { get; set; }
    }
}
