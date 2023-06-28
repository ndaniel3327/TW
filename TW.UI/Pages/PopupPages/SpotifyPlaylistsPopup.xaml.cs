using CommunityToolkit.Maui.Views;
using TW.UI.Constants;
using TW.UI.Models.Spotify.View;

namespace TW.UI.Pages.PopupPages;

public partial class SpotifyPlaylistsPopup : Popup
{
    private List<SpotifyPlaylistGroup> _playlists = new();
    public List<SpotifyPlaylistGroup> Playlists
    {
        get
        {
            return _playlists;
        }
        set
        {
            _playlists = value;
            OnPropertyChanged(nameof(Playlists));
        }
    }

    public SpotifyPlaylistsPopup(List<SpotifyPlaylistGroup> spotifyPlaylists)
    {
        Size= new Size(DeviceDisplay.Current.MainDisplayInfo.Width/3, DeviceDisplay.Current.MainDisplayInfo.Height/4);
        BindingContext = this;

        InitializeComponent();

        Playlists = spotifyPlaylists;
    }
}
