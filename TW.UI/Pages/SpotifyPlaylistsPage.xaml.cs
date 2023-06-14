using CommunityToolkit.Maui.Core.Extensions;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TW.UI.Models.Spotify.View;
using TW.UI.Services.Spotify;

namespace TW.UI.Pages;

public partial class SpotifyPlaylistsPage : ContentPage
{
    private readonly ISpotifyService _spotifyCService;
    private List<SpotifyPlaylistGroup> _playlists;
    public List<SpotifyPlaylistGroup> Playlists
    {
        get
        {
            return _playlists;
        }
        set
        {
            if (_playlists != value)
            {
                _playlists = value;
                OnPropertyChanged(nameof(Playlists));
            }
        }
    }

    public SpotifyPlaylistsPage(ISpotifyService spotifyCService)
    {
        InitializeComponent();

        _spotifyCService = spotifyCService;

        BindingContext = this;

        try
        {
            GetPlaylists();
        }
        catch(Exception ex)
        {
            Debug.WriteLine("//////////" + ex.Message);
        }
    }
    private async void GetPlaylists()
    {
        var playlistModels = await _spotifyCService.GetPlaylists();
        var playlistGroups = new List<SpotifyPlaylistGroup>();
        foreach (var playlist in playlistModels)
        {
            var playlistGroup = new SpotifyPlaylistGroup(playlist.Name, playlist.Tracks);
            playlistGroups.Add(playlistGroup);
        }
        Playlists = playlistGroups;
    }
}
