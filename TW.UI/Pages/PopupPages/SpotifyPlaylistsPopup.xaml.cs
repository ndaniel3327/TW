using CommunityToolkit.Maui.Views;
using TW.UI.Constants;
using TW.UI.Helpers;
using TW.UI.Models.Spotify.View;

namespace TW.UI.Pages.PopupPages;

public partial class SpotifyPlaylistsPopup : Popup
{
    private string[] _playlists ;
    private readonly Action _action;

    public string[] Playlists
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

    public SpotifyPlaylistsPopup(Action action)
    {
        Size= new Size(DeviceDisplay.Current.MainDisplayInfo.Width/3, DeviceDisplay.Current.MainDisplayInfo.Height/4);
        BindingContext = this;

        InitializeComponent();
        _action = action;

        var playlists = File.ReadAllLines(SpotifyConstants.SpotifyPlaylitsFileFullPath);
        string[] playlistNameArray = new string[playlists.Length];
        string[] playlistIdArray = new string[playlists.Length];

        int i = 0;
        foreach (var playlist in playlists)
        {
            string name = FileStorageHelper.ReturnName(playlist);
            string id = FileStorageHelper.ReturnId(playlist);
            playlistNameArray[i] = name;
            playlistIdArray[i] = id;
            i++;
        }
        Playlists = playlistNameArray;
    }

    private void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var items = ((CollectionView)sender).SelectedItems;
    }

    private void OnXButtonClicked(object sender, EventArgs e)
    {
        _action.Invoke();
        this.Close();
    }
}
