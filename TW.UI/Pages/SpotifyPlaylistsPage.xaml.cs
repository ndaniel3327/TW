using TW.UI.Services;

namespace TW.UI.Pages;

public partial class SpotifyPlaylistsPage : ContentPage
{
    private readonly ISpotifyCService _spotifyCService;
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

    public SpotifyPlaylistsPage(ISpotifyCService spotifyCService)
    {
        InitializeComponent();
        BindingContext = this;
        _spotifyCService = spotifyCService;
        GetPlaylists();
    }
    private async void GetPlaylists()
    {
        List<string> playlistsAsListString = await _spotifyCService.GetPlaylists();
        List<PlaylistModel>playlistsAsPlaylistModels = new List<PlaylistModel>();
        foreach (string playlistName in playlistsAsListString)
        {
            playlistsAsPlaylistModels.Add(new PlaylistModel { Name = playlistName});
        }
        Playlists=playlistsAsPlaylistModels;
    }
}
