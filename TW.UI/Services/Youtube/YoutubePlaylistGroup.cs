using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TW.UI.Services.Youtube
{
    public class YoutubePlaylistGroup
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
        [JsonPropertyName ("title")]
        public string Name { get; set; }
    }
}
