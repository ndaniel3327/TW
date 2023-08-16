using CommunityToolkit.Maui.Views;
using TW.UI.Helpers;

namespace TW.UI.Pages.PopupPages;

public partial class LocalPlaylistsPopup : Popup
{
    private List<PlaylistAndId> _playlists;
    private List<object> _selectedItems;

    private readonly Action _action;

    public List<PlaylistAndId> Playlists { get => _playlists; private set => _playlists = value; }
    public List<object> SelectedItems { get => _selectedItems; private set => _selectedItems = value; }

    public LocalPlaylistsPopup(Action action)
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
        var playlists = FileStorageHelper.ReadLocalPlaylistsFile();

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
        var playlists = FileStorageHelper.ReadLocalPlaylistsFile();

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

        FileStorageHelper.CreateLocalPlaylistsFile(temporaryPlaylistList);

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