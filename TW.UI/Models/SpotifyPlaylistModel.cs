namespace TW.UI.Models
{
    public class SpotifyPlaylistModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<SpotifyTrackModel> Tracks { get; set; }
    }
}

