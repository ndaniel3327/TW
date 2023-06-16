using System.Text.Json.Serialization;

namespace TW.UI.Services.Spotify
{
    public  class SpotifyTokenDetails
    {
        [JsonPropertyName("access_token")]
        public string SpotifyAccessToken { get; set; }
        [JsonPropertyName("refresh_token")]
        public string SpotifyRefreshToken { get; set; }
        [JsonPropertyName("expires_in")]
        public string SpotifyTokenExpiresIn { get; set; }
        // public static string SpotifyAccessTokenExpirationDate { get; set; }
    }
}
