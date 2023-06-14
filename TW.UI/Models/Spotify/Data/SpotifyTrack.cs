using System.Collections.Generic;

namespace TW.UI.Models.Spotify.Data
{
    public class SpotifyTrack
    {
        public List<SpotifyArtist> Artists { get; set; }

        public string Artist => string.Join(" and ", Artists.Select(c => c.Name).ToArray());

        public string Name { get; set; }
    }
}
