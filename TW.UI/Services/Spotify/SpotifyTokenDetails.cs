using System.Text.Json.Serialization;

namespace TW.UI.Services
{
    public  class SpotifyTokenDetails
    {
        [JsonPropertyName("access_token")]
        public string SpotifyAccessToken { get; set; }

        [JsonPropertyName("refresh_token")]
        public string SpotifyRefreshToken { get; set; }

        [JsonPropertyName("expires_in")]
        public int SpotifyTokenExpiresInSeconds { get; set; }

        [JsonPropertyName("token_type")]
        public string SpotifyTokenType { get; set; }
    }
}
