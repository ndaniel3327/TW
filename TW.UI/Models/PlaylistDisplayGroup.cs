using System.ComponentModel;
using static TW.UI.Constants.AppConstants;

namespace TW.UI.Models
{
    public class PlaylistDisplayGroup : List<PlaylistDisplayTrack>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public PlaylistSourceEnum Source { get; set; }
        public ImageSource ImageSource { get; set; }

        public PlaylistDisplayGroup(string id, string name, List<PlaylistDisplayTrack> tracks,PlaylistSourceEnum source,ImageSource imageSource) : base(tracks)
        {
            Id = id;
            Name = name;
            Source = source;
            ImageSource=imageSource;
        }
    }

    public class PlaylistDisplayTrack : INotifyPropertyChanged
    {
        public string PlaylistsId { get; set; }
        public string TrackId { get;set; }
        public List<string> ArtistsNames { get; set; } = new();
        public string Name { get; set; }
        public string Artists => string.Join(" and ", ArtistsNames);

        private bool _isSelected;
        private bool _menuIsVisible;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected))); ;
            }
        }

        public bool MenuIsVisible
        {
            get { return _menuIsVisible; }
            set
            {
                _menuIsVisible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MenuIsVisible))); ;
            }
        }
    }
}
