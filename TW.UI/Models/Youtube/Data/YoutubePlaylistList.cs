using System.Text.Json.Serialization;

namespace TW.UI.Models.Youtube.Data
{
    public class YoutubePlaylistList
    {
        [JsonPropertyName("items")]
        public List<YoutubePlaylist> Playlists { get; set; }
    }
    public class YoutubePlaylist
    {
        public string Id { get; set; }

        [JsonPropertyName("snippet")]
        public PlaylistInfo PlaylistInfo { get; set; }

        [JsonPropertyName("items")]
        public List<YoutubeTrack> Tracks { get; set; }
    }

    public class PlaylistInfo
    {
        [JsonPropertyName("title")]
        public string Name { get; set; }
    }

    public class YoutubeTrack
    {
        [JsonPropertyName("snippet")]
        public YoutubeTrackInfo TrackInfo { get; set; }
    }

    public class YoutubeTrackInfo
    {
        [JsonPropertyName("title")]
        public string Name { get; set; }
    }
}
