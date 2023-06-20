using System.Diagnostics;
using TW.UI.Models.Spotify.Data;
using TW.UI.Models.Spotify.View;
using TW.UI.Services.Spotify;

namespace TW.UI.Pages;

public partial class SpotifyPlaylistsPage : ContentPage
{
    private readonly ISpotifyService _spotifyCService;
    private List<SpotifyPlaylistGroup> _playlists = new();
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

    public SpotifyPlaylistsPage(ISpotifyService spotifyCService,List<SpotifyPlaylistGroup> playlistGroup)
    {
        BindingContext = this;

        InitializeComponent();

        _spotifyCService = spotifyCService;

        //GetPlaylists();
    }
    private async Task GetPlaylists()
    {
        List<SpotifyPlaylist> playlistModels = new List<SpotifyPlaylist>();
        playlistModels = await _spotifyCService.GetPlaylists();

        var playlistGroups = new List<SpotifyPlaylistGroup>();
        foreach (var playlist in playlistModels)
        {
            var playlistGroup = new SpotifyPlaylistGroup(playlist.Name, playlist.Tracks);
            playlistGroups.Add(playlistGroup);
        }
        Playlists = playlistGroups;
    }
}
