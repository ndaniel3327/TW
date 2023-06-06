        using System.Collections.ObjectModel;

namespace TW.UI.Models
{
    public class SpotifyPlaylistGroup : List<SpotifyTrackModel>
    {
        public string Name { get; set; }
        public SpotifyPlaylistGroup(string name, List<SpotifyTrackModel> tracks) : base(tracks)
        {
            Name = name;
        }
    }
}
