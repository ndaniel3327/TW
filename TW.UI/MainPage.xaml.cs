using CommunityToolkit.Maui.Views;
using TW.UI.Constants;
using TW.UI.Pages;
using TW.UI.Pages.PopupPages;
using TW.UI.Services.Spotify;
using TW.UI.Services.Youtube;

namespace TW.UI
{
    public partial class MainPage : ContentPage
    {
        private readonly ISpotifyService _spotifyService;
        private readonly IYoutubeService _youtubeService;

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
                _youtubeIsLoggedIn = value;
                ChangeYoutubeButtonVIsibilityAccordingToLoginStatus();
                CheckGoToPlaylistkButtonStatus();

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
                _spotifyIsLoggedIn = value;
                ChangeSpotifyButtonVisibilityAccordingToLoginStatus();
                CheckGoToPlaylistkButtonStatus();
            }
        }

        public MainPage(ISpotifyService spotifyService, IYoutubeService youtubeService)
        {
            InitializeComponent();
            CheckYoutubeLoginStatus();
            CheckSpotifyLoginStatus();
            _spotifyService = spotifyService;
            _youtubeService = youtubeService;
        }

        private async void OnSpotifyButtonClicked(object sender, EventArgs e)
        {
            if (SpotifyIsLoggedIn == true)
            {
                var playlistModels = await _spotifyService.GetPlaylists();

                await Task.Run(() => SpotifyConstants.playlistGroups = playlistModels);

                await Shell.Current.GoToAsync(nameof(SpotifyPlaylistsPage));
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

        private async void OnGoToPlaylistsButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(PlaylistsPage));
        }
        private async void OnAddLocalFilesButtonClicked(object sender, EventArgs e)
        {
            var audioFileType=new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                {DevicePlatform.Android,new[]{ "audio/mpeg"} }  //audio/mpeg means .mp3 files
            });

            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle="Pick Audio File Please",
                FileTypes=audioFileType
            });
            if (result == null)
                return;

            string fileName = result.FileName;
            string filePath = result.FullPath;

            var popup = new PlaylistSelectForLocalFilesPopup(fileName,filePath);

            this.ShowPopup(popup);
        }

        private async void CheckYoutubeLoginStatus()
        {
            var expirationDate = await SecureStorage.Default.GetAsync("YoutubeTokenExpirationDate");
            var refreshToken = await SecureStorage.Default.GetAsync("YoutubeRefreshToken");

            if (expirationDate != null && DateTime.Compare(DateTime.Parse(expirationDate), DateTime.Now) > 0)
            {
                YoutubeIsLoggedIn = true;
            }
            else if (expirationDate != null && DateTime.Compare(DateTime.Parse(expirationDate), DateTime.Now) < 0 && refreshToken != null)
            {
                bool isSuccess = await _youtubeService.RefreshAccessToken();
                if (isSuccess)
                {
                    YoutubeIsLoggedIn = true;
                }
                else
                {
                    YoutubeIsLoggedIn = false;
                }
            }
            else
            {
                YoutubeIsLoggedIn = false;
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
        private void ChangeSpotifyButtonVisibilityAccordingToLoginStatus()
        {
            if (SpotifyIsLoggedIn == true)
            {
                SpotifyButton.IsVisible = false;
            }
            else if (SpotifyIsLoggedIn == false)
            {
                SpotifyButton.IsVisible = true;
            }

        }
        private void ChangeYoutubeButtonVIsibilityAccordingToLoginStatus()
        {
            if (YoutubeIsLoggedIn == true)
            {
                YoutubeButton.IsVisible = false;
            }
            else if (YoutubeIsLoggedIn == false)
            {
                YoutubeButton.IsVisible = true;
            }
        }
        private async void CheckGoToPlaylistkButtonStatus()
        {
            if (SpotifyIsLoggedIn == true || YoutubeIsLoggedIn == true)
            {
                GoToPlaylistsButton.IsVisible = true;
            }
            else if (SpotifyIsLoggedIn == false && YoutubeIsLoggedIn == false)
            {
                GoToPlaylistsButton.IsVisible = false;
            }

            if (SpotifyIsLoggedIn == true && YoutubeIsLoggedIn == true)
            {
                await Shell.Current.GoToAsync(nameof(PlaylistsPage));
            }
        }

        private void LogOutYoutube_Clicked(object sender, EventArgs e)
        {
            YoutubeIsLoggedIn = false;
            SpotifyIsLoggedIn = false;
            SecureStorage.Default.RemoveAll();
        }
    }
}