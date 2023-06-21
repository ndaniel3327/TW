using TW.UI.Constants;
using TW.UI.Models.Spotify.View;

namespace TW.UI.Pages;

public partial class SpotifyPlaylistsPage : ContentPage
{
    private List<SpotifyPlaylistGroup> _playlists = new();
    public List<SpotifyPlaylistGroup> Playlists
    {
        get
        {
            return _playlists;
        }
        set
        {
            _playlists = value;
            OnPropertyChanged(nameof(Playlists));
        }
    }

    public SpotifyPlaylistsPage()
    {
        BindingContext = this;

        InitializeComponent();

        Playlists = SpotifyConstants.playlistGroups;
    }
}
