using System.Collections;
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
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        GetPlaylists();

    }
    private async void GetPlaylists()
    {
        List<string> playlists = await _spotifyCService.GetPlaylists();
        _playlists = new List<PlaylistModel>();
        foreach (string playlistName in playlists)
        {
            _playlists.Add(new PlaylistModel { Name = playlistName});
        }

    }
}
