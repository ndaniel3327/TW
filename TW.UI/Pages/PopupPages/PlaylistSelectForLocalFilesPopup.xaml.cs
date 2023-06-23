using CommunityToolkit.Maui.Views;
using TW.UI.Constants;

namespace TW.UI.Pages.PopupPages;

public partial class PlaylistSelectForLocalFilesPopup : Popup
{
    private string _mainDirectoryPath;
    public List<string> Playlists { get; set; }
    public PlaylistSelectForLocalFilesPopup(string fileName)
    {
        InitializeComponent();
        string mainDirectoryPath = FileSystem.Current.AppDataDirectory;
        _mainDirectoryPath = mainDirectoryPath;
        GetLocalPlaylists();
    }
    private void GetLocalPlaylists()
    {
        string fullPath = Path.Combine(_mainDirectoryPath, LocalFilesConstants.LocalPlaylistsFileName);
        if (File.Exists(fullPath))
        {
            string content = File.ReadAllText(fullPath);
            List<string> playlists = content.Split(",").ToList();
            Playlists = playlists;
        }
    }
}