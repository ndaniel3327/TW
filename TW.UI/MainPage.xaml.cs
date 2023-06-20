using System.Diagnostics;
using TW.UI.Constants;
using TW.UI.Models.Spotify.View;
using TW.UI.Pages;
using TW.UI.Services.Spotify;
using TW.UI.Services.Youtube;

namespace TW.UI
{
    public partial class MainPage : ContentPage
    {
        private readonly ISpotifyService _spotifyService;
        private readonly IYoutubeClientService _youtubeService;

        private bool _youtubeIsLoggedIn = false;
        private bool _spotifyIsLoggedIn = false;

        public bool YoutubeIsLoggedIn
        {
            get
            {
                return _youtubeIsLoggedIn;
            }
            set
            {
                if (value == true)
                {
                    ChangeYoutubeButtonStyleForLoggedInUser();
                }
                _youtubeIsLoggedIn = value;

            }
        }

        public bool SpotifyIsLoggedIn
        {
            get
            {
                return _spotifyIsLoggedIn;
            }
            set
            {

                if (value == true)
                {
                    ChangeSpotifyButtonStyleForLoggedInUser();
                }
                _spotifyIsLoggedIn = value;
            }
        }

        public MainPage(ISpotifyService spotifyService, IYoutubeClientService youtubeService)
        {
            InitializeComponent();
            //  CheckYoutubeLoginStatus();
            CheckSpotifyLoginStatus();
            _spotifyService = spotifyService;
            _youtubeService = youtubeService;
        }

        private async void OnSpotifyButtonClicked(object sender, EventArgs e)
        {
            if (SpotifyIsLoggedIn == true)
            {
                try
                {
                    var playlistModels = await _spotifyService.GetPlaylists();

                    var playlistGroups = new List<SpotifyPlaylistGroup>();
                    foreach (var playlist in playlistModels)
                    {
                        var playlistGroup = new SpotifyPlaylistGroup(playlist.Name, playlist.Tracks);
                        playlistGroups.Add(playlistGroup);
                    }
                    SpotifyConstants.playlistGroups = playlistGroups;

                    await Shell.Current.GoToAsync(nameof(SpotifyPlaylistsPage));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("//////// " + ex.Message);
                }
            }
            else
            {
                Uri loginUri = await _spotifyService.GetAuthorizationUri();

                BrowserLaunchOptions options = new BrowserLaunchOptions()
                {
                    LaunchMode = BrowserLaunchMode.SystemPreferred,

                };
                await Browser.Default.OpenAsync(loginUri, options);
            }
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
                _youtubeIsLoggedIn = true;
            }
            else if (expirationDate != null && DateTime.Compare(DateTime.Parse(expirationDate), DateTime.Now) < 0 && refreshToken != null)
            {
                _youtubeService.RefreshAccessToken();
                _youtubeIsLoggedIn = true;
            }
            else
            {
                _youtubeIsLoggedIn = false;
            }
        }
        private async void CheckSpotifyLoginStatus()
        {
            var tokenExpirationDate = await SecureStorage.Default.GetAsync(SpotifyConstants.StorageNameTokenExpirationDate);

            if (tokenExpirationDate != null && DateTime.Compare(DateTime.Parse(tokenExpirationDate), DateTime.Now) > 0)
            {
                SpotifyIsLoggedIn = true;
            }
            else if (tokenExpirationDate != null && DateTime.Compare(DateTime.Parse(tokenExpirationDate), DateTime.Now) < 0)
            {
                bool isSuccess = await _spotifyService.RefreshAccessToken();
                if (isSuccess)
                {
                    SpotifyIsLoggedIn = true;
                }
                else
                {
                    SpotifyIsLoggedIn = false;
                }
            }
            else
            {
                SpotifyIsLoggedIn = false;
            }
        }
        private void ChangeSpotifyButtonStyleForLoggedInUser()
        {
            SpotifyButton.BackgroundColor = Colors.AntiqueWhite;
            SpotifyButton.Text = "SpotifyPlaylists";
            SpotifyButton.TextColor = Colors.Gray;
        }
        private void ChangeYoutubeButtonStyleForLoggedInUser()
        {
            YoutubeButton.BackgroundColor = Colors.AntiqueWhite;
            YoutubeButton.Text = "YoutubePlaylists";
            YoutubeButton.TextColor = Colors.Gray;
        }


        private void LogOutYoutube_Clicked(object sender, EventArgs e)
        {
            YoutubeIsLoggedIn = false;
            SpotifyIsLoggedIn = false;
            SecureStorage.Default.RemoveAll();
        }
    }
}