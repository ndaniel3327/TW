namespace TW.UI.Models
{
    public class PlaylistDisplayGroup : List<PlaylistDisplayTracks>
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public PlaylistDisplayGroup()
        {
            
        }
        public PlaylistDisplayGroup(string id, string name, List<PlaylistDisplayTracks> tracks) : base(tracks)
        {
            Id = id;
            Name = name;
        }
    }

    public class PlaylistDisplayTracks
    {
        public List<string> ArtistsNames { get; set; }
        public string Name { get; set; }
        public string Artists { get; set; }
    }
}
