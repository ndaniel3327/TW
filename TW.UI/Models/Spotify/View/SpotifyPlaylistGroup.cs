namespace TW.UI.Models.Spotify.View
{
    public class SpotifyPlaylistGroup : List<SpotifyTrackView>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<SpotifyTrackView> Tracks { get; set; }

        public SpotifyPlaylistGroup(string id, string name , List<SpotifyTrackView> tracks) : base(tracks)
        {
            Id = id;
            Name= name;
            Tracks = tracks;

        }
    }
}
