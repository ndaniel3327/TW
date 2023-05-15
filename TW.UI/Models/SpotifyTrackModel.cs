namespace TW.UI.Models
{
    public class SpotifyTrackModel
    {
        public List<SpotifyArtistModel> Artists { get; set; }

        public string Artist => string.Join(" and ", Artists);

        public string Name { get; set; }
    }
}

