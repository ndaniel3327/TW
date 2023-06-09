using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TW.UI.Services.Spotify
{
    public class SpotifyTokenDetails
    {
        [JsonPropertyName("accessToken")]
        public string SpotifyAccessToken { get; set; }
        [JsonPropertyName("refreshToken")]
        public string SpotifyRefreshToken { get; set; }
        [JsonPropertyName("tokenType")]
        public string SpotifyTokenType { get; set; }
        [JsonPropertyName("expiresInSeconds")]
        public int SpotifyTokenExpiresInSeconds { get; set; }
        [JsonPropertyName("createdAt")]
        public DateTime SpotifyTokenCreatedAtDate { get; set; }
        public DateTime SpotifyTokenExpirationDate { get; set; }
    }
}
