using System.ComponentModel;
using TW.UI.Services;

namespace TW.UI.Pages;

public partial class SpotifyPlaylistsPage : ContentPage , INotifyPropertyChanged
{
    private readonly ISpotifyCService _spotifyCService;
    private IEnumerable<string> _playlists;   
    public IEnumerable<string> Playlists
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
            }
        }
    }

    public SpotifyPlaylistsPage(ISpotifyCService spotifyCService)
	{
		InitializeComponent();
        BindingContext = this;
        _spotifyCService = spotifyCService;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _playlists = await GetPlaylists();

    }
    private async Task<List<string>> GetPlaylists()
    {
        return await _spotifyCService.GetPlaylists();
    }
}