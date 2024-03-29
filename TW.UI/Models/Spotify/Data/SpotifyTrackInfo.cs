﻿using System.Text.Json.Serialization;

namespace TW.UI.Models.Spotify.Data
{
    public class SpotifyTrackInfo
    {
        [JsonPropertyName("artists")]
        public List<SpotifyArtist> Artists { get; set; }

        //public string Artist => string.Join(" and ", Artists.Select(c => c.Name).ToArray());

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("album")]
        public SpotifyAlbum SpotifyAlbum { get; set; }
    }

    public class SpotifyAlbum
    {
        [JsonPropertyName("images")]
        public List<SpotifyImage> SpotifyImages { get; set; }
    }
}