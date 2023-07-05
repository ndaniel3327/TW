using CommunityToolkit.Maui.Views;
using TW.UI.Constants;
using TW.UI.Helpers;
using TW.UI.Models;
using TW.UI.Pages.PopupPages;
using TW.UI.Services.Spotify;
using TW.UI.Services.Youtube;

namespace TW.UI.Pages;

public partial class PlaylistsPage : ContentPage
{
    private string _selected = "true"; //default selected value
    //private string _deselected = "false";

    private readonly ISpotifyService _spotifyService;
    private readonly IYoutubeService _youtubeService;

    private List<PlaylistDisplayGroup> _spotifyPlaylistGroupsData;
    private List<PlaylistDisplayGroup> _youtubePlaylistGroupsData;

    private Action RefreshSpotifyDisplayedItemsDelegate;
    private Action RefreshYoutubeDisplayedItemsDelegate;

    private List<PlaylistDisplayGroup> _displayedSpotifyPlaylists = new();
    public List<PlaylistDisplayGroup> DisplayedSpotifyPlaylists
    {
        get
        {
            return _displayedSpotifyPlaylists;
        }
        set
        {
            _displayedSpotifyPlaylists = value;
            DisplayedPlaylists = _displayedSpotifyPlaylists.Concat(_displayedYoutubePlaylists).ToList();
        }
    }
    private List<PlaylistDisplayGroup> _displayedYoutubePlaylists=new();
    public List<PlaylistDisplayGroup> DisplayedYoutubePlaylists
    {
        get
        {
            return _displayedYoutubePlaylists;
        }
        set
        {
            _displayedYoutubePlaylists = value;
            DisplayedPlaylists = _displayedSpotifyPlaylists.Concat(_displayedYoutubePlaylists).ToList();
        }
    }
    private List<PlaylistDisplayGroup> _displayedPlaylists = new();
    public List<PlaylistDisplayGroup> DisplayedPlaylists 
    {
        get 
        {
            return _displayedPlaylists;
        }
        set
        {
            _displayedPlaylists = value;
            OnPropertyChanged(nameof(DisplayedPlaylists));
        }
    }


    public PlaylistsPage(MainPage mainPage, ISpotifyService spotifyService, IYoutubeService youtubeService)
    {
        BindingContext = this;
        InitializeComponent();

        RefreshSpotifyDisplayedItemsDelegate = GetDisplayedSpotifyPlaylists;
        RefreshYoutubeDisplayedItemsDelegate = GetDisplayedYoutubePlaylists;

        _spotifyService = spotifyService;
        _youtubeService = youtubeService;

        if (mainPage.IsSpotifyLoggedIn == true)
        {
            GetSpotifyPlaylistData();
        }
        else
        {
            spotifyButton.IsEnabled = false;
            spotifyButton.BackgroundColor = Colors.Gray;
        }

        if(mainPage.IsYoutubeLoggedIn==true)
        {
            GetYoutubePlaylistData();
        }
        else if (mainPage.IsYoutubeLoggedIn == false)
        {
            youtubeButton.IsEnabled = false;
            youtubeButton.BackgroundColor = Colors.Gray;
        }
    }

    private async void GetYoutubePlaylistData()
    {
        await Task.Run(async() =>
        {
            _youtubePlaylistGroupsData = await _youtubeService.GetYoutubePlaylists();
            var youtubePlaylistsStorageData = new List<string>();

            if (!File.Exists(YoutubeConstants.YoutubePlaylitsFileFullPath))
            {
                var stream = File.Create(YoutubeConstants.YoutubePlaylitsFileFullPath);
                stream.Close();

                foreach (var playlist in _youtubePlaylistGroupsData)
                {
                    youtubePlaylistsStorageData.Add(FileStorageHelper.GenerateAndReturnEntry(playlist.Id,playlist.Name,_selected));
                }

                File.WriteAllLines(YoutubeConstants.YoutubePlaylitsFileFullPath, youtubePlaylistsStorageData.ToArray());

                DisplayedYoutubePlaylists = _youtubePlaylistGroupsData;
            }
            else
            {
                var oldYoutubePlaylistsStorageData = File.ReadAllLines(YoutubeConstants.YoutubePlaylitsFileFullPath).ToList();
                if (oldYoutubePlaylistsStorageData.Count == 0)
                {
                    foreach (var playlist in _youtubePlaylistGroupsData)
                    {
                        youtubePlaylistsStorageData.Add(FileStorageHelper.GenerateAndReturnEntry(playlist.Id, playlist.Name, _selected));
                    }

                    File.WriteAllLines(YoutubeConstants.YoutubePlaylitsFileFullPath, youtubePlaylistsStorageData.ToArray());

                    DisplayedYoutubePlaylists = _youtubePlaylistGroupsData;
                }
                else if (oldYoutubePlaylistsStorageData.Count > 0)
                {
                    foreach (var playlist in _youtubePlaylistGroupsData)
                    {
                        bool isNew = true;
                        foreach (var oldPlaylist in oldYoutubePlaylistsStorageData)
                        {
                            string oldPlaylistId = FileStorageHelper.ReturnId(oldPlaylist);
                            if (oldPlaylistId == playlist.Id)
                            {
                                youtubePlaylistsStorageData.Add(oldPlaylist);
                                isNew = false;
                            }

                        }
                        if (isNew)
                        {
                            youtubePlaylistsStorageData.Add(FileStorageHelper.GenerateAndReturnEntry(playlist.Id, playlist.Name, _selected));
                        }
                    }
                    File.WriteAllLines(YoutubeConstants.YoutubePlaylitsFileFullPath, youtubePlaylistsStorageData.ToArray());
                    GetDisplayedYoutubePlaylists();
                }
            }
        });
    }

    private void GetDisplayedYoutubePlaylists()
    {
        var playlists = File.ReadAllLines(YoutubeConstants.YoutubePlaylitsFileFullPath).ToList();
        List<string> selectedPlaylistsIds = new();
        foreach (var playlist in playlists)
        {
            string isSelected = FileStorageHelper.ReturnSelected(playlist);
            if (isSelected == "true")
            {
                selectedPlaylistsIds.Add(FileStorageHelper.ReturnId(playlist));
            }
        }

        var youtubePlaylistGroupsDisplay = new List<PlaylistDisplayGroup>();
        foreach (var playlist in _youtubePlaylistGroupsData)
        {
            foreach (var id in selectedPlaylistsIds)
            {
                if (playlist.Id == id)
                {
                    youtubePlaylistGroupsDisplay.Add(playlist);
                }
            }
        }
        DisplayedYoutubePlaylists = youtubePlaylistGroupsDisplay;
    }

    private async void GetSpotifyPlaylistData()
    {
        await Task.Run(async () =>
        {
            _spotifyPlaylistGroupsData = await _spotifyService.GetPlaylists();
            var spotifyPlaylistsStorageData = new List<string>();

            if (!File.Exists(SpotifyConstants.SpotifyPlaylitsFileFullPath))
            {
                var stream = File.Create(SpotifyConstants.SpotifyPlaylitsFileFullPath);
                stream.Close();

                foreach (var playlist in _spotifyPlaylistGroupsData)
                {
                    spotifyPlaylistsStorageData.Add(FileStorageHelper.GenerateAndReturnEntry(playlist.Id,playlist.Name,_selected));
                }

                File.WriteAllLines(SpotifyConstants.SpotifyPlaylitsFileFullPath, spotifyPlaylistsStorageData.ToArray());

                DisplayedSpotifyPlaylists = _spotifyPlaylistGroupsData;
            }
            else
            {
                var oldSpotifyPlaylistsStorageData = File.ReadAllLines(SpotifyConstants.SpotifyPlaylitsFileFullPath).ToList();
                if (oldSpotifyPlaylistsStorageData.Count == 0)
                {
                    foreach (var playlist in _spotifyPlaylistGroupsData)
                    {
                        spotifyPlaylistsStorageData.Add(FileStorageHelper.GenerateAndReturnEntry(playlist.Id,playlist.Name,_selected));
                    }

                    File.WriteAllLines(SpotifyConstants.SpotifyPlaylitsFileFullPath, spotifyPlaylistsStorageData.ToArray());

                    DisplayedSpotifyPlaylists = _spotifyPlaylistGroupsData;
                }
                else if (oldSpotifyPlaylistsStorageData.Count > 0)
                {
                    foreach (var playlist in _spotifyPlaylistGroupsData)
                    {
                        bool isNew = true;
                        foreach (var oldPlaylist in oldSpotifyPlaylistsStorageData)
                        {
                            string oldPlaylistId = FileStorageHelper.ReturnId(oldPlaylist);
                            if (oldPlaylistId == playlist.Id)
                            {
                                spotifyPlaylistsStorageData.Add(oldPlaylist);
                                isNew = false;
                            }

                        }
                        if (isNew)
                        {
                            spotifyPlaylistsStorageData.Add(FileStorageHelper.GenerateAndReturnEntry(playlist.Id, playlist.Name, _selected));
                        }
                    }
                    File.WriteAllLines(SpotifyConstants.SpotifyPlaylitsFileFullPath, spotifyPlaylistsStorageData.ToArray());
                    GetDisplayedSpotifyPlaylists();
                }
            }
        });
    }
    private void GetDisplayedSpotifyPlaylists()
    {
        var playlists = File.ReadAllLines(SpotifyConstants.SpotifyPlaylitsFileFullPath).ToList();
        List<string> selectedPlaylistsIds = new();
        foreach (var playlist in playlists)
        {
            string isSelected = FileStorageHelper.ReturnSelected(playlist);
            if (isSelected == "true")
            {
                selectedPlaylistsIds.Add(FileStorageHelper.ReturnId(playlist));
            }
        }

        var spotifyPlaylistGroupsDisplay = new List<PlaylistDisplayGroup>();
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
    private void OnYoutubeButtonClicked(object sender, EventArgs e)
    {
        this.ShowPopup(new YoutubePlaylistsPopup(RefreshYoutubeDisplayedItemsDelegate));
    }

    private void OnSpotifyButtonClicked(object sender, EventArgs e)
    {
        this.ShowPopup(new SpotifyPlaylistsPopup(RefreshSpotifyDisplayedItemsDelegate));
    }

    private void OnLocalButtonClicked(object sender, EventArgs e)
    {

    }
}