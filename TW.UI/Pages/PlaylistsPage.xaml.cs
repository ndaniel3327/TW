using CommunityToolkit.Maui.Views;
using TW.UI.Constants;
using TW.UI.Helpers;
using TW.UI.Models.Spotify.View;
using TW.UI.Pages.PopupPages;
using TW.UI.Services.Spotify;

namespace TW.UI.Pages;

public partial class PlaylistsPage : ContentPage
{
    private readonly ISpotifyService _spotifyService;

    private List<string> _spotifyPlaylistsStorageData=new();

    private List<SpotifyPlaylistGroup> _spotifyPlaylistGroupsData;
    private List<SpotifyPlaylistGroup> _displayedSpotifyPlaylists;
    private Action RefreshDisplayedItemsDelegate;
    public List<SpotifyPlaylistGroup> DisplayedSpotifyPlaylists
    {
        get
        {
            return _displayedSpotifyPlaylists;
        }
        set
        {
            _displayedSpotifyPlaylists = value;
            OnPropertyChanged(nameof(DisplayedSpotifyPlaylists));
        }
    }


    public PlaylistsPage(MainPage mainPage, ISpotifyService spotifyService)
    {
        InitializeComponent();

        RefreshDisplayedItemsDelegate = GetDisplayedSpotifyPlaylists;

        _spotifyService = spotifyService;

        if (mainPage.IsSpotifyLoggedIn == true)
        {
            GetSpotifyPlaylistData();
        }
        else
        {
            spotifyButton.IsEnabled = false;
            spotifyButton.BackgroundColor = Colors.Gray;
        }

        if (mainPage.IsYoutubeLoggedIn == false)
        {
            youtubeButton.IsEnabled = false;
            youtubeButton.BackgroundColor = Colors.Gray;
        }
    }

    private void SpotifyButtonClicked(object sender, EventArgs e)
    {
        this.ShowPopup(new SpotifyPlaylistsPopup(RefreshDisplayedItemsDelegate));
    }

    private async void GetSpotifyPlaylistData()
    {
        _spotifyPlaylistGroupsData = await _spotifyService.GetPlaylists();
        //var spotifyPlaylistsStorageData = new List<string>();

        string spotifyPlaylistsFilePath = SpotifyConstants.SpotifyPlaylitsFileFullPath;

        if (!File.Exists(spotifyPlaylistsFilePath))
        {
            var stream = File.Create(spotifyPlaylistsFilePath);
            stream.Close();

            foreach (var playlist in _spotifyPlaylistGroupsData)
            {
                _spotifyPlaylistsStorageData.Add("id=" + playlist.Id + "@name=" + playlist.Name + "@selected=true");
            }

            File.WriteAllLines(spotifyPlaylistsFilePath, _spotifyPlaylistsStorageData.ToArray());

            DisplayedSpotifyPlaylists = _spotifyPlaylistGroupsData;
        }
        else
        {
            var oldSpotifyPlaylistsStorageData = File.ReadAllLines(spotifyPlaylistsFilePath).ToList();
            if (oldSpotifyPlaylistsStorageData.Count == 0)
            {
                foreach (var playlist in _spotifyPlaylistGroupsData)
                {
                    _spotifyPlaylistsStorageData.Add("id=" + playlist.Id + "@name=" + playlist.Name + "@selected=true");
                }

                File.WriteAllLines(spotifyPlaylistsFilePath, _spotifyPlaylistsStorageData.ToArray());

                DisplayedSpotifyPlaylists = _spotifyPlaylistGroupsData;
            }
            else if (oldSpotifyPlaylistsStorageData.Count > 0)
            {
                foreach (var playlist in _spotifyPlaylistGroupsData)
                {
                    foreach (var oldPlaylist in oldSpotifyPlaylistsStorageData)
                    {
                        string oldPlaylistId = FileStorageHelper.ReturnId(oldPlaylist);
                        if (oldPlaylistId == playlist.Id)
                        {
                            _spotifyPlaylistsStorageData.Add(oldPlaylist);
                        }
                        else if (oldPlaylistId != playlist.Id)
                        {
                            _spotifyPlaylistsStorageData.Add("id=" + playlist.Id + "@name=" + playlist.Name + "@selected=true");
                        }
                    }
                }
                File.WriteAllLines(spotifyPlaylistsFilePath, _spotifyPlaylistsStorageData.ToArray());
                GetDisplayedSpotifyPlaylists();
            }
            
        }

    }
    private void GetDisplayedSpotifyPlaylists()
    {
        List<string> selectedPlaylistsIds = new();
        foreach (var playlist in _spotifyPlaylistsStorageData)
        {
            string isSelected = FileStorageHelper.ReturnSelected(playlist);
            if (isSelected == "true")
            {
                selectedPlaylistsIds.Add(FileStorageHelper.ReturnId(playlist));
            }
        }

        var spotifyPlaylistGroupsDisplay = new List<SpotifyPlaylistGroup>();
        foreach (var playlist in _spotifyPlaylistGroupsData)
        {
            foreach (var id in selectedPlaylistsIds)
            {
                if (playlist.Id == id)
                {
                    spotifyPlaylistGroupsDisplay.Add(playlist);
                }
            }
        }
        DisplayedSpotifyPlaylists = spotifyPlaylistGroupsDisplay;
    }
    private void YoutubeButtonClicked(object sender, EventArgs e)
    {

    }

    private void LocalButtonClicked(object sender, EventArgs e)
    {

    }
}