using TW.UI.Enums;

namespace TW.UI.Models
{
    public class PlaylistDisplayGroup : List<PlaylistDisplayTracks>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public PlaylistSource Source { get; set; }
        public ImageSource ImageSource { get; set; }

        public PlaylistDisplayGroup()
        {
            
        }
        public PlaylistDisplayGroup(string id, string name, List<PlaylistDisplayTracks> tracks,PlaylistSource source,ImageSource imageSource) : base(tracks)
        {
            Id = id;
            Name = name;
            Source = source;
            ImageSource=imageSource;
        }
    }

    public class PlaylistDisplayTracks
    {
        public List<string> ArtistsNames { get; set; }
        public string Name { get; set; }
        public string Artists { get; set; }
    }
}
