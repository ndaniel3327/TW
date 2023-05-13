
using CommunityToolkit.Maui.Views;
using TW.UI.Pages;
using TW.UI.Services;

namespace TW.UI
{
    public partial class MainPage : ContentPage
    {
        private readonly ISpotifyClientService _spotifyService;

        public delegate void PopupDelegate();
        public MainPage(ISpotifyClientService spotifyService)
        {

            InitializeComponent();

            _spotifyService = spotifyService;
        }
        private async void PopupClosed()
        {
            await Shell.Current.GoToAsync(nameof(SpotifyPlaylistsPage));
        }

        private async void OnSpotifyButtonClicked(object sender, EventArgs e)
        {
                Uri loginUri = await _spotifyService.AuthorizeSpotify();
                PopupDelegate popupDelegate = PopupClosed;
                this.ShowPopup(new SpotifyAuthorizationPopup(loginUri, popupDelegate));
        }
    }
}