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

        private bool _youtubeIsLoggedIn = false;
        private bool _spotifyIsLoggedIn = false;

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
            if (_youtubeIsLoggedIn == true)
            {
                await Shell.Current.GoToAsync(nameof(YoutubePlaylistsPage));
            }
            else
            {
                Uri loginUri = _youtubeService.GetAuthorizationLink();
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
            var expirationDate = await SecureStorage.Default.GetAsync("YoutubeTokenExpirationDate");
            var refreshToken = await SecureStorage.Default.GetAsync("YoutubeRefreshToken");

            if (expirationDate != null && DateTime.Compare(DateTime.Parse(expirationDate), DateTime.Now) > 0)
            {
                _youtubeIsLoggedIn=true;
                ChangeButtonStyleForLoggedInUser("YoutubePlaylists");
            }
            else if(expirationDate != null && DateTime.Compare(DateTime.Parse(expirationDate), DateTime.Now) < 0 && refreshToken != null)
            {
                 _youtubeService.RefreshAccessToken();
                _youtubeIsLoggedIn = true;
                ChangeButtonStyleForLoggedInUser("YoutubePlaylists");

            }
        }
        private async void CheckSpotifyLoginStatus()
        {
            var expirationDate = await SecureStorage.Default.GetAsync("SpotifyTokenExpirationDate");
            var refreshToken = await SecureStorage.Default.GetAsync("SpotifyRefreshToken");

            if (expirationDate != null && DateTime.Compare(DateTime.Parse(expirationDate), DateTime.Now) > 0)
            {
                _spotifyIsLoggedIn = true;
                ChangeButtonStyleForLoggedInUser("SpotifyPlaylists");
            }
            else if (expirationDate != null && DateTime.Compare(DateTime.Parse(expirationDate), DateTime.Now) < 0 && refreshToken != null)
            {
                var isSuccess = await _spotifyService.RefreshAccessToken();
                if (isSuccess)
                {
                    _spotifyIsLoggedIn = true;
                    ChangeButtonStyleForLoggedInUser("SpotifyPlaylists");
                }
            }
        }
        void ChangeButtonStyleForLoggedInUser(string text)
        {
            YoutubeButton.BackgroundColor = Colors.AntiqueWhite;
            YoutubeButton.Text = text;
            YoutubeButton.TextColor = Colors.Gray;
        }

        private void LogOutYoutube_Clicked(object sender, EventArgs e)
        {
            _youtubeIsLoggedIn = false;
            SecureStorage.Default.RemoveAll();
        }
    }
}