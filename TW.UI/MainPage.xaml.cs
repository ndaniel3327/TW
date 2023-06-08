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

        private bool _isLoggedIn = false;

        public delegate void PopupDelegate();
        public MainPage(ISpotifyClientService spotifyService, IYoutubeClientService youtubeService)
        {

            InitializeComponent();
            CheckYoutubeLoginStatus();
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
            if (_isLoggedIn == true)
            {
                await Shell.Current.GoToAsync(nameof(YoutubePlaylistsPage));
            }
            else
            {
                Uri loginUri = _youtubeService.AuthorizeYoutube();
                //this.ShowPopup(new YoutubeAuthorizationPopup(loginUri));
                BrowserLaunchOptions options = new BrowserLaunchOptions()
                {
                    LaunchMode = BrowserLaunchMode.SystemPreferred,

                };
                await Browser.Default.OpenAsync(loginUri, options);
            }
        }
        private async void CheckYoutubeLoginStatus()
        {
            var expirationDate = await SecureStorage.Default.GetAsync("ExpirationDate");
            var addingDate = await SecureStorage.Default.GetAsync("AddingDate");
            if (expirationDate != null && addingDate != null && DateTime.Compare(DateTime.Parse(expirationDate), DateTime.Now) > 0)
            {
                _isLoggedIn=true;
                YoutubeButton.BackgroundColor = Colors.AntiqueWhite;
                YoutubeButton.Text = "PlaylistPage";
                YoutubeButton.TextColor = Colors.Gray;
            }
        }
    }
}