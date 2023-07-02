using CommunityToolkit.Maui.Views;
using TW.UI.Constants;
using TW.UI.Helpers;

namespace TW.UI.Pages.PopupPages;

public partial class SpotifyPlaylistsPopup : Popup
{
    private readonly Action _action;

    
    private List<object> _preselectedItems;
    public List<object> PreselectedItems
    {
        get => _preselectedItems;
        set
        {
            _preselectedItems = value;
            OnPropertyChanged(nameof(PreselectedItems));
        }
    }

    private List<PlaylistAndId> _playlists;
    private List<PlaylistAndId> _curentlySelectedItems;

    public List<PlaylistAndId>Playlists
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

        var playlistNameAndId = new List<PlaylistAndId>();
        List<object> preselectedItems = new();
        int i = 0;
        foreach (var playlist in playlists)
        {
            string name = FileStorageHelper.ReturnName(playlist);
            string id = FileStorageHelper.ReturnId(playlist);
            string selected = FileStorageHelper.ReturnSelected(playlist);
            if (selected == "true")
            {
                preselectedItems.Add(new PlaylistAndId { Name = name, Id = id });
            }
            playlistNameAndId.Add(new PlaylistAndId { Name = name, Id = id });
            i++;
        }
        _preselectedItems = preselectedItems;
        Playlists = playlistNameAndId;
    }

    private void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        //var items = ((CollectionView)sender).SelectedItems;
        var selectedItems = e.CurrentSelection;
        List<PlaylistAndId> castedSelectedItems = (List<PlaylistAndId>)selectedItems;
        _curentlySelectedItems = castedSelectedItems;

        //foreach (var selectedItem in castedSelectedItems)
        //{
        //    bool isNew = true;
        //    foreach (var item in _selectedItems)
        //    {
        //        if (item.Id == selectedItem.Id)
        //        {
        //            isNew = false;
        //        }
        //    }
        //    if(isNew)
        //    {
        //        _selectedItems.Add(new PlaylistAndId { Id = selectedItem.Id, Name = selectedItem.Name });
        //    }
        //}
    }

    private void OnXButtonClicked(object sender, EventArgs e)
    {
        var playlists = File.ReadAllLines(SpotifyConstants.SpotifyPlaylitsFileFullPath);

        List<PlaylistAndId> temporarySelectedItemsList = new();

        List<string> newPlaylistData = new();
        foreach (var item in _curentlySelectedItems)
        {
            bool isNew= true;
            foreach (var playlistItem in playlists)
            {
                string id = FileStorageHelper.ReturnId(playlistItem);
                if(id == item.Id)
                {
                    newPlaylistData.Add(FileStorageHelper.GenerateAndReturnEntry(item.Id, item.Name, "true"));
                    isNew = false;
                }
            }
            if(isNew)
            {
                newPlaylistData.Add(FileStorageHelper.GenerateAndReturnEntry(item.Id, item.Name, "false"));
            }
        }
        _action.Invoke();
        this.Close();
    }
}
