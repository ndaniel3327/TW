using AutoMapper;
using CommunityToolkit.Maui.Views;
using TW.UI.Constants;
using TW.UI.Helpers;
using TW.UI.Models;
using TW.UI.Models.Spotify.View;
using TW.UI.Pages.PopupPages;
using TW.UI.Services.Spotify;

namespace TW.UI.Pages;

public partial class PlaylistsPage : ContentPage
{
    private readonly IMapper _mapper;
    private readonly ISpotifyService _spotifyService;

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


    public PlaylistsPage(MainPage mainPage, IMapper mapper, ISpotifyService spotifyService)
    {
        BindingContext = this;
        InitializeComponent();

        RefreshDisplayedItemsDelegate = GetDisplayedSpotifyPlaylists;
        _mapper = mapper;
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
        var mapped = _mapper.Map<PlaylistDisplayGroup>(DisplayedSpotifyPlaylists);
    }

    private void SpotifyButtonClicked(object sender, EventArgs e)
    {
        this.ShowPopup(new SpotifyPlaylistsPopup(RefreshDisplayedItemsDelegate));
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
                    spotifyPlaylistsStorageData.Add("id=" + playlist.Id + "@name=" + playlist.Name + "@selected=true");
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
                        spotifyPlaylistsStorageData.Add("id=" + playlist.Id + "@name=" + playlist.Name + "@selected=true");
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
                            spotifyPlaylistsStorageData.Add("id=" + playlist.Id + "@name=" + playlist.Name + "@selected=true");
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