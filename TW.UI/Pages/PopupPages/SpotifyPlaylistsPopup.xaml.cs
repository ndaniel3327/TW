using CommunityToolkit.Maui.Views;
using TW.UI.Constants;
using TW.UI.Models.Spotify.View;

namespace TW.UI.Pages.PopupPages;

public partial class SpotifyPlaylistsPopup : Popup
{
    private List<string> _playlists = new();
    public List<string> Playlists
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

    public SpotifyPlaylistsPopup(List<string> spotifyPlaylistNames)
    {
        Size= new Size(DeviceDisplay.Current.MainDisplayInfo.Width/3, DeviceDisplay.Current.MainDisplayInfo.Height/4);
        BindingContext = this;

        InitializeComponent();

        Playlists = spotifyPlaylistNames;
    }

    private void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void OnXButtonClicked(object sender, EventArgs e)
    {
        this.Close();
    }
}
