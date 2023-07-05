using CommunityToolkit.Maui.Views;
using TW.UI.Constants;

namespace TW.UI.Pages.PopupPages;

public partial class PlaylistSelectForLocalFilesPopup : Popup
{
    private string _mainDirectoryPath;
    private List<string> _playlists;
    private Size _popupSize;
    private readonly string _fileName;
    private readonly string _filePath;
    private string _selectedPlaylist;
    private int _selectedItemIndex;

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
    public Size PopupSize
    {
        get
        {
            return _popupSize;
        }
        set
        {
            _popupSize = value;
            OnPropertyChanged(nameof(PopupSize));
        }
    }
    public PlaylistSelectForLocalFilesPopup(string fileName, string filePath)
    {
        var screenWith = DeviceDisplay.MainDisplayInfo.Width;
        var screenHeight = DeviceDisplay.MainDisplayInfo.Height;
        PopupSize = new Size(screenWith, screenHeight);

        InitializeComponent();
        BindingContext = this;

        string mainDirectoryPath = FileSystem.Current.AppDataDirectory;
        _mainDirectoryPath = mainDirectoryPath;
        GetLocalPlaylists();
        _fileName = fileName;
        _filePath = filePath;
    }
    private int GetLocalPlaylists()
    {
        if (File.Exists(LocalFilesConstants.LocalPlaylistsFileFullPath))
        {
            string[] playlistNames = File.ReadAllLines(LocalFilesConstants.LocalPlaylistsFileFullPath);
            //List<string> playlists = new();
            //foreach (string line in content)
            //{
            //    int startingIndex = line.IndexOf("name=" + 1);
            //    int endingIndex = line.IndexOf('&');
            //    string playlistName = line.Substring(line.IndexOf ("name=") + 1,endingIndex-startingIndex);
            //    playlists.Add(playlistName);
            //}
            Playlists = playlistNames.ToList();
            return Playlists.Count;
        }
        else
        {
            var stream = File.Create(LocalFilesConstants.LocalPlaylistsFileFullPath);
            stream.Close();
            return 0;
        }
    }

    private void OnNewPlaylistButtonClicked(object sender, EventArgs e)
    {
        myEntry.IsVisible = true;
    }

    private void OnOkButtonClicked(object sender, EventArgs e)
    {
        var playlistFilePath = Path.Combine(_mainDirectoryPath, _selectedPlaylist);
        if (!File.Exists(playlistFilePath))
        {
            var stream = File.Create(playlistFilePath);
            stream.Close();
        }

        File.AppendAllText(playlistFilePath, "name=" + _fileName + "@path=" + _filePath + Environment.NewLine);
        var content = File.ReadAllLines(playlistFilePath);

        this.Close();
    }

    private void OnEntryCompleted(object sender, EventArgs e)
    {
        string playlistName = ((Entry)sender).Text;
        if( !string.IsNullOrEmpty(playlistName))
        {
            string fullPath = LocalFilesConstants.LocalPlaylistsFileFullPath;
            File.AppendAllText(fullPath, playlistName + Environment.NewLine);

            string mainDirectoryPath = FileSystem.Current.AppDataDirectory;
            var stream = File.Create(Path.Combine(mainDirectoryPath, playlistName));
            stream.Close();

            myEntry.Text = String.Empty;

            GetLocalPlaylists();
            okButton.IsVisible= false;
            deletePlaylistButton.IsVisible= false;
        } 
    }

    private void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        okButton.IsVisible = true;
        deletePlaylistButton.IsVisible = true;
        if(e.SelectedItem!= null && e.SelectedItemIndex>=0) 
        {
            _selectedPlaylist = e.SelectedItem.ToString();
            _selectedItemIndex = e.SelectedItemIndex;
        }
    }

    private void OnDeletePlaylistButtonClicked(object sender, EventArgs e)
    {
        string fullPath = LocalFilesConstants.LocalPlaylistsFileFullPath;
        var playlistNames = File.ReadAllLines(fullPath);

        List<string> temporaryPlaylistNameList = new List<string>(playlistNames);
        temporaryPlaylistNameList.RemoveAt(_selectedItemIndex);
        File.WriteAllLines(fullPath,temporaryPlaylistNameList.ToArray());

        File.Delete(Path.Combine(_mainDirectoryPath, _selectedPlaylist));

        int numberOfPlaylists = GetLocalPlaylists();
        if(numberOfPlaylists == 0)
        {
            okButton.IsVisible=false;
            deletePlaylistButton.IsVisible = false;
        }
        listView.SelectedItem = null;
        okButton.IsVisible = false;
        deletePlaylistButton.IsVisible = false;  
    }
}