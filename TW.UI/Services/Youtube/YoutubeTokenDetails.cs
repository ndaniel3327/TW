using System.Text.Json.Serialization;

namespace TW.UI.Services
{
    public class YoutubeTokenDetails
    {
        [JsonPropertyName("access_token")]
        public string YoutubeAccessToken { get; set; }

        [JsonPropertyName("refresh_token")]
        public string YoutubeRefreshToken { get; set; }

        [JsonPropertyName("token_type")]
        public string YoutubeTokenType { get; set; }

        [JsonPropertyName("expires_in")]
        public int YoutubeExpiresInSeconds { get; set; }
    }
}
