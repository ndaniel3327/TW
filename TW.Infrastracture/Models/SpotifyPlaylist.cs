using System.Collections.Generic;

namespace TW.Infrastructure.Models
{
    public class SpotifyPlaylist
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<SpotifyTrack> Tracks { get; set; }
    }
}
