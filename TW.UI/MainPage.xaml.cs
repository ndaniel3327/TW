using CommunityToolkit.Maui.Views;
using TW.UI.Pages;
using TW.UI.Services.Spotify;
using TW.UI.Services.Youtube;

namespace TW.UI
{
    public partial class MainPage : ContentPage
    {
        private readonly ISpotifyClientService _spotifyService;
        private readonly IYoutubeClientService _youtubeService;

        public delegate void PopupDelegate();
        public MainPage(ISpotifyClientService spotifyService, IYoutubeClientService youtubeService)
        {

            InitializeComponent();

            _spotifyService = spotifyService;
            _youtubeService = youtubeService;
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
        private async void OnYoutubeButtonClicked(object sender, EventArgs e)
        {
            Uri loginUri = _youtubeService.AuthorizeYoutube();
            //this.ShowPopup(new YoutubeAuthorizationPopup(loginUri));
            BrowserLaunchOptions options = new BrowserLaunchOptions()
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,

            };
            await Browser.Default.OpenAsync(loginUri, options);
        }

        private async void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(YoutubePlaylistsPage));
        }
    }
}