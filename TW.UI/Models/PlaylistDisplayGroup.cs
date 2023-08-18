using System.ComponentModel;
using static TW.UI.Constants.AppConstants;

namespace TW.UI.Models
{
    public class PlaylistDisplayGroup : List<PlaylistDisplayTrack>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public PlaylistSourceEnum Source { get; set; }
        //public List<PlayerImage> PlayerImages { get; set; }
        public ImageSource GroupImageSource { get; set; }

        public PlaylistDisplayGroup(string id, string name, List<PlaylistDisplayTrack> tracks, PlaylistSourceEnum source, ImageSource imageSource) : base(tracks)
        {
            Id = id;
            Name = name;
            Source = source;
            GroupImageSource = imageSource;
           // PlayerImages = playerImages;
        }
    }

    public class PlayerImage
    {
        public string Url { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }

    public class PlaylistDisplayTrack : INotifyPropertyChanged
    {


        private ImageSource _popupPlayerImage;

        public ImageSource PopupPlayerImage
        {
            get { return _popupPlayerImage; }
            set { _popupPlayerImage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }

        public string PlaylistsId { get; set; }
        public string TrackId { get; set; }
        public List<string> ArtistsNames { get; set; } = new();
        public string Name { get; set; }
        public string Artists => string.Join(" and ", ArtistsNames);

        private bool _isSelected;
        private bool _menuIsVisible;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected))); 
            }
        }

        public bool MenuIsVisible
        {
            get { return _menuIsVisible; }
            set
            {
                _menuIsVisible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MenuIsVisible))); 
            }
        }
    }
}
