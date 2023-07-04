namespace TW.UI.Models
{
    public class PlaylistDisplayGroup : List<PlaylistDisplayTracks>
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public List<PlaylistDisplayTracks> Tracks { get; set; }

        public PlaylistDisplayGroup()
        {
            
        }
        public PlaylistDisplayGroup(string id, string name, List<PlaylistDisplayTracks> tracks) : base(tracks)
        {
            Id = id;
            Name = name;
            Tracks = tracks;
        }
    }

    public class PlaylistDisplayTracks
    {
        public PlaylistDisplayTracks()
        {

        }
        public PlaylistDisplayTracks(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Artists { get; set; }
    }
}
