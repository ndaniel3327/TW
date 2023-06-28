using CommunityToolkit.Maui.Views;
using System.Collections.Generic;
using TW.UI.Models.Spotify.View;
using TW.UI.Pages.PopupPages;
using TW.UI.Services.Spotify;

namespace TW.UI.Pages;

public partial class PlaylistsPage : ContentPage
{
    private readonly bool _spotifyLoginStatus;
    private readonly bool _youtubeLoginStatus;
    private readonly ISpotifyService _spotifyService;

    public PlaylistsPage(MainPage mainPage, ISpotifyService spotifyService)
    {
        InitializeComponent();
        _spotifyLoginStatus = mainPage.SpotifyIsLoggedIn;
        _youtubeLoginStatus = mainPage.YoutubeIsLoggedIn;
        //TODO:remove this
        //_spotifyLoginStatus = false;
        if (_spotifyLoginStatus == false)
        {
            spotifyButton.IsEnabled = false;
            spotifyButton.BackgroundColor = Colors.Gray;
        }

        if (_youtubeLoginStatus == false)
        {
            youtubeButton.IsEnabled = false;
            youtubeButton.BackgroundColor = Colors.Gray;
        }
        _spotifyService = spotifyService;
    }

    private async void SpotifyButtonClicked(object sender, EventArgs e)
    {
        List<SpotifyPlaylistGroup> spotifyPlaylists = await _spotifyService.GetPlaylists();
        this.ShowPopup(new SpotifyPlaylistsPopup(spotifyPlaylists));
    }

    private void YoutubeButtonClicked(object sender, EventArgs e)
    {

    }

    private void LocalButtonClicked(object sender, EventArgs e)
    {

    }
}