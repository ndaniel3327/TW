using System.ComponentModel;
using static TW.UI.Constants.AppConstants;

namespace TW.UI.Models
{
    public class PlaylistDisplayGroupModel : List<PlaylistDisplayTrack>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public PlaylistSourceEnum Source { get; set; }

        public ImageSource GroupImageSource { get; set; }

        public PlaylistDisplayGroupModel(string id, string name, List<PlaylistDisplayTrack> tracks, PlaylistSourceEnum source, ImageSource imageSource) : base(tracks)
        {
            Id = id;
            Name = name;
            Source = source;
            GroupImageSource = imageSource;
        }
    }

    public class PlaylistDisplayTrack : INotifyPropertyChanged
    {
        public string PlaylistsId { get; set; }
        public string TrackId { get; set; }
        public List<string> ArtistsNames { get; set; } = new();
        public string Name { get; set; }
        public string Artists => string.Join(" and ", ArtistsNames);

        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }
        private bool _menuIsVisible;
        public bool MenuIsVisible
        {
            get { return _menuIsVisible; }
            set
            {
                _menuIsVisible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MenuIsVisible)));
            }
        }

        private string _popupPlayerImageUri;
        public string PopupPlayerImageUri {
            get { return _popupPlayerImageUri; }
            set
            {
                _popupPlayerImageUri = value;

            }
        }

        public  ImageSource PopupPlayerImage
        {
            get
            {
                var client = new HttpClient();
                if (_popupPlayerImageUri != null)
                {
                    var stream = Task.Run(async () => await client.GetStreamAsync(_popupPlayerImageUri)).Result;
                    return ImageSource.FromStream(() => stream);
                }
                return ImageSource.FromFile("noimage.svg");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
