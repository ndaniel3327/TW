using CommunityToolkit.Maui.Core.Extensions;
using System.Runtime.CompilerServices;
using TW.UI.Models;
using TW.UI.Services;

namespace TW.UI.Pages;

public partial class SpotifyPlaylistsPage : ContentPage
{
    private readonly ISpotifyClientService _spotifyCService;
    private List<SpotifyPlaylistModel> _playlists;
    public List<SpotifyPlaylistModel> Playlists
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

    public SpotifyPlaylistsPage(ISpotifyClientService spotifyCService)
    {
        InitializeComponent();

        _spotifyCService = spotifyCService;

        BindingContext = this;

        GetPlaylists();
    }
    private async void GetPlaylists()
    {
        var playlistModels = await _spotifyCService.GetPlaylists();
        //var playlistGroups = new List<SpotifyPlaylistGroup>();
        //foreach (var playlist in playlistModels)
        //{
        //    var playlistGroup = new SpotifyPlaylistGroup(playlist.Name, playlist.Playlists);
        //    playlistGroups.Add(playlistGroup);
        //}
        Playlists =playlistModels;
    }
}
