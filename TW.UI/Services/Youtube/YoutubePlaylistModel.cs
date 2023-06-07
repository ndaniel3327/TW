namespace TW.UI.Services.Youtube
{
    public class YoutubePlaylistModel : List<YoutubeTrackModel>
    {    
        public string Name { get; set; }

        public YoutubePlaylistModel(string name, List<YoutubeTrackModel> tracks) : base(tracks)
        {
            Name = name;
        }

    }
}
