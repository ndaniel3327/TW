using System.ComponentModel;

namespace TW.UI.Models
{
    public class SpotifyTrackModel 
    {
        public List<SpotifyArtistModel> Artists { get; set; }
        public string Artist => string.Join(" and ", Artists.Select(c=>c.Name).ToArray());
        public string Name { get; set; }



    }
}

