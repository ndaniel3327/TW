using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TW.UI.Models.Spotify.Data
{
    public class SpotifyTrack
    {
        [JsonPropertyName("track")]
        public SpotifyTrackInfo TrackInfo { get; set; }
    }
}
