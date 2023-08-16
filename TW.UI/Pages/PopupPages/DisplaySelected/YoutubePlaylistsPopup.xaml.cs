using CommunityToolkit.Maui.Views;
using TW.UI.Helpers;

namespace TW.UI.Pages.PopupPages;

public partial class YoutubePlaylistsPopup : Popup
{
    private readonly Action _action;

    private List<PlaylistAndId> _playlists;
    public List<PlaylistAndId> Playlists
    {
        get
        {
            return _playlists;
        }
        set
        {
            if (_playlists != value)
            {
                _playlists = value;
                OnPropertyChanged(nameof(Playlists));
            }
        }
    }
    private List<object> _selectedItems;

    public List<object> SelectedItems
    {
        get
        {
            return _selectedItems;
        }
        set
        {
            _selectedItems = value;
            OnPropertyChanged();
        }
    }


    public YoutubePlaylistsPopup(Action action)
    {
        Playlists = new List<PlaylistAndId>();

        Size = new Size(DeviceDisplay.Current.MainDisplayInfo.Width / 3, DeviceDisplay.Current.MainDisplayInfo.Height / 4);

        BindingContext = this;
        _action = action;

        GetAllItemsAndPreselectedItems();

        InitializeComponent();
    }

    private void GetAllItemsAndPreselectedItems()
    {
        var playlists = FileStorageHelper.ReadYoutubePlaylistsFile();

        foreach (var playlist in playlists)
        {
            string name = FileStorageHelper.ReturnName(playlist);
            string id = FileStorageHelper.ReturnId(playlist);
            var isSelected = FileStorageHelper.ReturnIsSelected(playlist);

            Playlists.Add(new PlaylistAndId { Name = name, Id = id, IsSelected = isSelected });
        }
        var preselected = Playlists.Where(x => x.IsSelected);
        SelectedItems = new List<object>();
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
        var playlists = FileStorageHelper.ReadYoutubePlaylistsFile();

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
                        temporaryPlaylistList.Add(FileStorageHelper.GenerateAndReturnEntry(id, name));
                        isNotSelected = false;
                    }
                }
                if (isNotSelected)
                {
                    temporaryPlaylistList.Add(FileStorageHelper.GenerateAndReturnEntry(id, name, false));
                }
            }
        }
        else if (items == null || items.Count == 0)
        {
            foreach (var playlist in playlists)
            {
                string id = FileStorageHelper.ReturnId(playlist);
                string name = FileStorageHelper.ReturnName(playlist);
                temporaryPlaylistList.Add(FileStorageHelper.GenerateAndReturnEntry(id, name, false));
            }
        }

        FileStorageHelper.CreateYoutubePlaylistsFile(temporaryPlaylistList);
    }

    private void OnXMarkButtonClicked(object sender, EventArgs e)
    {
        this.Close();
    }

    private void OnPopupClosed(object sender, CommunityToolkit.Maui.Core.PopupClosedEventArgs e)
    {
        _action.Invoke();
    }
}