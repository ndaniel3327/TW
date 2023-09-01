using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TW.UI.Models.Spotify.Data
{
    public class SpotifyPlaylistList
    {
        [JsonPropertyName("items")]
        public List<SpotifyPlaylistLIstItem> Playlists { get; set; }
    }
}
