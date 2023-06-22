namespace TW.UI.Models.Youtube.View
{
    public class YoutubePlaylistGroup : List<YoutubeTrackView>
    {
        public string Name { get; set; }

        public YoutubePlaylistGroup(string name, List<YoutubeTrackView> tracks) : base(tracks)
        {
            Name = name;
        }

    }
}
