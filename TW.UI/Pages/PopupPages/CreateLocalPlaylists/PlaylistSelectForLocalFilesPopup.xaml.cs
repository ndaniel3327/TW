using CommunityToolkit.Maui.Views;
using TW.UI.Helpers;
using TW.UI.Pages.PopupPages;

namespace TW.UI.Pages.PopupPages;

public partial class PlaylistSelectForLocalFilesPopup : Popup
{
    private string _selectedPlaylistId;
    private int _selectedItemIndex;

    private readonly string _fileName;
    private readonly string _filePath;

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
    public PlaylistSelectForLocalFilesPopup(string fileName, string filePath)
    {
        var screenWith = DeviceDisplay.MainDisplayInfo.Width;
        var screenHeight = DeviceDisplay.MainDisplayInfo.Height;
        this.Size = new Size(screenWith, screenHeight);

        _fileName = fileName;
        _filePath = filePath;

        InitializeComponent();

        BindingContext = this;

        GetLocalPlaylists();
    }
    private int GetLocalPlaylists()
    {
        if (FileStorageHelper.LocalPlaylitsFileExists())
        {
            var localPlaylists = FileStorageHelper.ReadLocalPlaylistsFile();

            List<PlaylistAndId> playlistsList = new();
            foreach (string localPlaylist in localPlaylists)
            {
                string id = FileStorageHelper.ReturnId(localPlaylist);
                string name = FileStorageHelper.ReturnName(localPlaylist);
                playlistsList.Add(new PlaylistAndId() { Id=id,Name=name});
            }

            Playlists = playlistsList;
            return Playlists.Count;
        }

        return 0;
    }

    private void OnNewPlaylistButtonClicked(object sender, EventArgs e)
    {
        myEntry.IsVisible = true;
    }

    private void OnOkButtonClicked(object sender, EventArgs e)
    {
        FileStorageHelper.UpdateLocalPlaylistSongsFile(_selectedPlaylistId, _fileName, _filePath);

        this.Close();
    }

    private void OnEntryCompleted(object sender, EventArgs e)
    {
        string playlistName = ((Entry)sender).Text;
        if (!string.IsNullOrEmpty(playlistName))
        {
            FileStorageHelper.UpdateLocalPlaylistsFile(playlistName);

            myEntry.Text = String.Empty;

            GetLocalPlaylists();

            okButton.IsVisible = false;
            deletePlaylistButton.IsVisible = false;
        }
    }

    private void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        okButton.IsVisible = true;
        deletePlaylistButton.IsVisible = true;

        if (e.SelectedItem != null && e.SelectedItemIndex >= 0)
        {
            _selectedPlaylistId = ((PlaylistAndId)e.SelectedItem).Id.ToString();
            _selectedItemIndex = e.SelectedItemIndex;
        }
    }

    private void OnDeletePlaylistButtonClicked(object sender, EventArgs e)
    {
        var playlistNames = FileStorageHelper.ReadLocalPlaylistsFile();

        var temporaryPlaylistNameList = new List<string>(playlistNames);
        temporaryPlaylistNameList.RemoveAt(_selectedItemIndex);

        FileStorageHelper.CreateLocalPlaylistsFile(temporaryPlaylistNameList);
        FileStorageHelper.DeleteLocalPlaylistSongsFile(_selectedPlaylistId);

        int numberOfPlaylists = GetLocalPlaylists();
        if (numberOfPlaylists == 0)
        {
            okButton.IsVisible = false;
            deletePlaylistButton.IsVisible = false;
        }

        listView.SelectedItem = null;
        okButton.IsVisible = false;
        deletePlaylistButton.IsVisible = false;
    }
}