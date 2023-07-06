using TW.UI.Enums;

namespace TW.UI.Models
{
    public class PlaylistDisplayGroup : List<PlaylistDisplayTrack>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public PlaylistSource Source { get; set; }
        public ImageSource ImageSource { get; set; }

        public PlaylistDisplayGroup(string id, string name, List<PlaylistDisplayTrack> tracks,PlaylistSource source,ImageSource imageSource) : base(tracks)
        {
            Id = id;
            Name = name;
            Source = source;
            ImageSource=imageSource;
        }
    }

    public class PlaylistDisplayTrack
    {
        public List<string> ArtistsNames { get; set; } = new();
        public string Name { get; set; }
        public string Artists => string.Join(" and ", ArtistsNames);
        public ImageSource MenuImageSource { get; set; }
    }
}
