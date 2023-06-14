using System.Collections.Generic;

namespace TW.Infrastructure.Models
{
    public class SpotifyTrack
    {
        public List<SpotifyArtist> Artists { get; set; }

        public string Artist => string.Join(" and ",Artists);

        public string Name { get; set; }
    }
}
