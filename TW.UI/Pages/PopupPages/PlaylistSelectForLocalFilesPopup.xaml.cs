using CommunityToolkit.Maui.Views;
using TW.UI.Constants;
using TW.UI.Helpers;

namespace TW.UI.Pages.PopupPages;

public partial class PlaylistSelectForLocalFilesPopup : Popup
{
    private string _selectedPlaylistId;
    private int _selectedItemIndex;
    private string _mainDirectoryPath;
    private string _fileName;
    private string _filePath;
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
        _mainDirectoryPath = FileSystem.Current.AppDataDirectory;
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
        var playlistFilePath = Path.Combine(_mainDirectoryPath, _selectedPlaylistId);
        if (!File.Exists(playlistFilePath))
        {
            var stream = File.Create(playlistFilePath);
            stream.Close();
        }

        string id = Guid.NewGuid().ToString();
        File.AppendAllText(playlistFilePath, "id=" + id + "@name=" + _fileName + "@path=" + _filePath + Environment.NewLine);
        var content = File.ReadAllLines(playlistFilePath);

        this.Close();
    }

    private void OnEntryCompleted(object sender, EventArgs e)
    {
        string playlistName = ((Entry)sender).Text;
        if (!string.IsNullOrEmpty(playlistName))
        {
            string fullPath = Path.Combine(_mainDirectoryPath, "LocalPlaylistsFile");
            string id = Guid.NewGuid().ToString();
            File.AppendAllText(fullPath, "id=" + id + "@name=" + playlistName +"@selected=true"+ Environment.NewLine);

            string mainDirectoryPath = FileSystem.Current.AppDataDirectory;
            var stream = File.Create(Path.Combine(mainDirectoryPath, id));
            stream.Close();

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

        List<string> temporaryPlaylistNameList = new List<string>(playlistNames);
        temporaryPlaylistNameList.RemoveAt(_selectedItemIndex);

        FileStorageHelper.CreateLocalPlaylistsFile(temporaryPlaylistNameList);

        File.Delete(Path.Combine(_mainDirectoryPath, _selectedPlaylistId));

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