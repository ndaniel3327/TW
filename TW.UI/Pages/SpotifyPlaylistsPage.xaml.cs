using TW.UI.Services;

namespace TW.UI.Pages;

public partial class SpotifyPlaylistsPage : ContentPage
{
    private readonly ISpotifyClientService _spotifyCService;
    private List<PlaylistModel> _playlists;   
    public List<PlaylistModel> Playlists
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
        var playlistsAsListString = await _spotifyCService.GetPlaylists();
        var playlistsAsPlaylistModels = new List<PlaylistModel>();

        foreach (string playlistName in playlistsAsListString)
        {
            playlistsAsPlaylistModels.Add(new PlaylistModel { Name = playlistName });
        }

        Playlists =playlistsAsPlaylistModels;
    }
}
