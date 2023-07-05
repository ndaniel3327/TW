using CommunityToolkit.Maui.Views;
using TW.UI.Constants;
using TW.UI.Helpers;

namespace TW.UI.Pages.PopupPages;

public partial class SpotifyPlaylistsPopup : Popup
{
    private readonly Action _action;

    private List<object> _selectedItems;
    public List<object> SelectedItems
    {
        get => _selectedItems;
        set
        {
            _selectedItems = value;
            OnPropertyChanged(nameof(SelectedItems));
        }
    }

    private List<PlaylistAndId> _playlists;

    public List<PlaylistAndId> Playlists
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
        Playlists = new List<PlaylistAndId>();
        SelectedItems = new List<object>();

        Size = new Size(DeviceDisplay.Current.MainDisplayInfo.Width / 3, DeviceDisplay.Current.MainDisplayInfo.Height / 4);
        BindingContext = this;
        _action = action;

        GetAllItemsAndPreselectedItems();
        InitializeComponent();
    }

    private void GetAllItemsAndPreselectedItems()
    {
        var playlists = File.ReadAllLines(SpotifyConstants.SpotifyPlaylitsFileFullPath);

        foreach (var playlist in playlists)
        {
            string name = FileStorageHelper.ReturnName(playlist);
            string id = FileStorageHelper.ReturnId(playlist);
            string selected = FileStorageHelper.ReturnSelected(playlist);
            bool isSelected = bool.Parse(selected);

            Playlists.Add(new PlaylistAndId { Name = name, Id = id, IsSelected = isSelected });
        }
        var preselected = Playlists.Where(x => x.IsSelected);

        for (int i = 0; i < Playlists.Count(); i++)
        {
            var playlist = Playlists[i];
            if (playlist.IsSelected)
            {
                SelectedItems.Add(Playlists[i]);
            }
        }
    }
    private void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var playlists = File.ReadAllLines(SpotifyConstants.SpotifyPlaylitsFileFullPath);

        var temporaryPlaylistList = new List<string>();
        var items = SelectedItems;
        if (items != null && items.Count != 0)
        {

            foreach (var playlist in playlists)
            {
                string id = FileStorageHelper.ReturnId(playlist);
                string name = FileStorageHelper.ReturnName(playlist);

                bool isNotSelected = true;
                foreach (var item in items)
                {
                    if (((PlaylistAndId)item).Id == id)
                    {
                        temporaryPlaylistList.Add(FileStorageHelper.GenerateAndReturnEntry(id, name, "true"));
                        isNotSelected = false;
                    }
                }
                if (isNotSelected)
                {
                    temporaryPlaylistList.Add(FileStorageHelper.GenerateAndReturnEntry(id, name, "false"));
                }
            }
        }
        else if (items == null || items.Count == 0)
        {
            foreach (var playlist in playlists)
            {
                string id = FileStorageHelper.ReturnId(playlist);
                string name = FileStorageHelper.ReturnName(playlist);
                temporaryPlaylistList.Add(FileStorageHelper.GenerateAndReturnEntry(id, name, "false"));
            }
        }
        File.WriteAllLines(SpotifyConstants.SpotifyPlaylitsFileFullPath, temporaryPlaylistList);
    }

    private void OnXButtonClicked(object sender, EventArgs e)
    {
        this.Close();
    }

    private void OnPopupClosed(object sender, CommunityToolkit.Maui.Core.PopupClosedEventArgs e)
    {
        _action.Invoke();
    }
}
