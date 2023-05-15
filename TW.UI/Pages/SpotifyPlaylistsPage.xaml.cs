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
        Playlists = await _spotifyCService.GetPlaylists();
    }
}
