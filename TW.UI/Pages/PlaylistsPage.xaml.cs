using Android.Util;
using CommunityToolkit.Maui.Views;
using TW.UI.Constants;
using TW.UI.Models.Spotify.View;
using TW.UI.Pages.PopupPages;
using TW.UI.Services.Spotify;

namespace TW.UI.Pages;

public partial class PlaylistsPage : ContentPage
{
    private readonly bool _spotifyLoginStatus;
    private readonly bool _youtubeLoginStatus;

    private readonly ISpotifyService _spotifyService;

    private string _mainDirectoryPath;
    //private string spotifyPlaylistsFilePath;

    private List<string> _spotifyPlaylistNames;

    private List<SpotifyPlaylistGroup> _spotifyPlaylists;

    public List<SpotifyPlaylistGroup> SpotifyPlaylists
    {
        get 
        {
            return _spotifyPlaylists; 
        }
        set
        { 
            _spotifyPlaylists = value;
            OnPropertyChanged(nameof(SpotifyPlaylists));
        }
    }


    public PlaylistsPage(MainPage mainPage, ISpotifyService spotifyService)
    {
        InitializeComponent();
        _spotifyLoginStatus = mainPage.SpotifyIsLoggedIn;
        _youtubeLoginStatus = mainPage.YoutubeIsLoggedIn;

        _spotifyService = spotifyService;

        _mainDirectoryPath = FileSystem.Current.AppDataDirectory;

        if (_spotifyLoginStatus == true)
        {
            GetSpotifyPlaylists();
        }
        else
        {
            spotifyButton.IsEnabled = false;
            spotifyButton.BackgroundColor = Colors.Gray;
        }

        if (_youtubeLoginStatus == false)
        {
            youtubeButton.IsEnabled = false;
            youtubeButton.BackgroundColor = Colors.Gray;
        }
    }

    private void SpotifyButtonClicked(object sender, EventArgs e)
    {
        this.ShowPopup(new SpotifyPlaylistsPopup(_spotifyPlaylistNames));
    }
    private async void GetSpotifyPlaylists()
    {
        var spotifyPlaylistGroupsData = await _spotifyService.GetPlaylists();
        _spotifyPlaylistNames = new List<string>();

        string spotifyPlaylistsFilePath = Path.Combine(_mainDirectoryPath, SpotifyConstants.SpotifyPlaylistsFileName);

        if (!File.Exists(spotifyPlaylistsFilePath))
        {
            foreach (var playlist in spotifyPlaylistGroupsData)
            {
                _spotifyPlaylistNames.Add("id=" + playlist.Id + "@name=" + playlist.Name + "@selected=false");
            }
            File.Create(spotifyPlaylistsFilePath);
            File.WriteAllLines(spotifyPlaylistsFilePath, _spotifyPlaylistNames.ToArray());
        }
        else
        {
            _spotifyPlaylistNames = File.ReadAllLines(spotifyPlaylistsFilePath).ToList();
        }
        List<string> selectedPlaylistsIds = new();
        foreach (var playlist in _spotifyPlaylistNames)
        {
            if (playlist.Substring(playlist.IndexOf("selected=") + 1) == "true")
            {
                selectedPlaylistsIds.Add(
                    playlist.Substring(
                        playlist.IndexOf("id=")+1,playlist.Substring(playlist.IndexOf("@name=")-5).Length));
            }
        }

        var spotifyPlaylistGroupsDisplay = new List<SpotifyPlaylistGroup>();
        foreach(var playlist in spotifyPlaylistGroupsData)
        {
            foreach(var id in selectedPlaylistsIds)
            {
                if (playlist.Id == id)
                {
                    spotifyPlaylistGroupsDisplay.Add(playlist);
                }
            }
        }
        SpotifyPlaylists = spotifyPlaylistGroupsDisplay;

    }
    private void YoutubeButtonClicked(object sender, EventArgs e)
    {

    }

    private void LocalButtonClicked(object sender, EventArgs e)
    {

    }
}