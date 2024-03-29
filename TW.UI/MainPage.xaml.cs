﻿using CommunityToolkit.Maui.Views;
using System.Globalization;
using TW.UI.Constants;
using TW.UI.Pages;
using TW.UI.Pages.PopupPages;
using TW.UI.Services;

namespace TW.UI
{
    public partial class MainPage : ContentPage
    {
        private readonly ISpotifyService _spotifyService;
        private readonly IYoutubeService _youtubeService;

        private readonly ISpeechToText _speechToText;

        private readonly CancellationTokenSource TokenSource = new ();

        private bool _youtubeIsLoggedIn = false;
        private bool _spotifyIsLoggedIn = false;
        private Command _listenCommand;

        public Command ListenCommand
        {
            get => _listenCommand;
            set 
            { 
                _listenCommand = value;
                OnPropertyChanged(nameof(ListenCommand));
            }
        }
        public Command ListenCancelCommand { get; set; }
        public string RecognitionText { get; set; }

        public bool IsYoutubeLoggedIn
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
        public bool IsSpotifyLoggedIn
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

        public MainPage(ISpotifyService spotifyService, 
            IYoutubeService youtubeService, 
            ISpeechToText speechToText)
        {
            BindingContext = this;

            InitializeComponent();
            _spotifyService = spotifyService;
            _youtubeService = youtubeService;
            _speechToText = speechToText;
            CheckYoutubeLoginStatus();
            CheckSpotifyLoginStatus();
            //TODO: test full functionality while having 0 playlists in the Spotify/Youtube account

            ListenCommand = new Command(Listen);
            ListenCancelCommand = new Command(ListenCancel);
        }
        private async void Listen()
        {
            var isAuthorized = await _speechToText.RequestPermissions();
            if (isAuthorized)
            {
                try
                {
                    RecognitionText = await _speechToText.Listen(CultureInfo.GetCultureInfo("ro-RO"),
                        new Progress<string>(partialText =>
                        {
                            if (DeviceInfo.Platform == DevicePlatform.Android)
                            {
                                RecognitionText = partialText;
                            }
                            else
                            {
                                RecognitionText += partialText + " ";
                            }

                            OnPropertyChanged(nameof(RecognitionText));
                        }), TokenSource.Token);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "OK");
                }
            }
            else
            {
                await DisplayAlert("Permission Error", "No microphone access", "OK");
            }
        }
        private void ListenCancel()
        {
            TokenSource?.Cancel();
        }

        private async void OnSpotifyButtonClicked(object sender, EventArgs e)
        {
            if (IsSpotifyLoggedIn == true)
            {
                var playlistModels = await _spotifyService.GetPlaylists();

               // await Task.Run(() => SpotifyConstants.playlistGroups = playlistModels);

               // await Shell.Current.GoToAsync(nameof(SpotifyPlaylistsPage));
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
            if (IsYoutubeLoggedIn == true)
            {
                // await Shell.Current.GoToAsync(nameof(YoutubePlaylistsPage));
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
                {DevicePlatform.Android,new[]{ "audio/mpeg"} } 
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
            var expirationDate = await SecureStorage.Default.GetAsync(YoutubeConstants.StorageNameTokenExpirationDate);
            var refreshToken = await SecureStorage.Default.GetAsync(YoutubeConstants.StorageNameRefreshToken);

            if (expirationDate != null && DateTime.Compare(DateTime.Parse(expirationDate), DateTime.Now) > 0)
            {
                IsYoutubeLoggedIn = true;
            }
            else if (expirationDate != null && DateTime.Compare(DateTime.Parse(expirationDate), DateTime.Now) < 0 && refreshToken != null)
            {
                bool isSuccess = await _youtubeService.RefreshAccessToken();
                if (isSuccess)
                {
                    IsYoutubeLoggedIn = true;
                }
                else
                {
                    IsYoutubeLoggedIn = false;
                }
            }
            else
            {
                IsYoutubeLoggedIn = false;
            }
        }

        private async void CheckSpotifyLoginStatus()
        {
            var tokenExpirationDate = await SecureStorage.Default.GetAsync(SpotifyConstants.StorageNameTokenExpirationDate);

            if (tokenExpirationDate != null && DateTime.Compare(DateTime.Parse(tokenExpirationDate), DateTime.Now) > 0)
            {
                IsSpotifyLoggedIn = true;
            }
            else if (tokenExpirationDate != null && DateTime.Compare(DateTime.Parse(tokenExpirationDate), DateTime.Now) < 0)
            {
                bool isSuccess = await _spotifyService.RefreshAccessToken();
                if (isSuccess)
                {
                    IsSpotifyLoggedIn = true;
                }
                else
                {
                    IsSpotifyLoggedIn = false;
                }
            }
            else
            {
                IsSpotifyLoggedIn = false;
            }
        }
        private void ChangeSpotifyButtonVisibilityAccordingToLoginStatus()
        {
            if (IsSpotifyLoggedIn == true)
            {
                SpotifyButton.IsVisible = false;
            }
            else if (IsSpotifyLoggedIn == false)
            {
                SpotifyButton.IsVisible = true;
            }

        }
        private void ChangeYoutubeButtonVIsibilityAccordingToLoginStatus()
        {
            if (IsYoutubeLoggedIn == true)
            {
                YoutubeButton.IsVisible = false;
            }
            else if (IsYoutubeLoggedIn == false)
            {
                YoutubeButton.IsVisible = true;
            }
        }
        private async void CheckGoToPlaylistkButtonStatus()
        {
            if (IsSpotifyLoggedIn == true || IsYoutubeLoggedIn == true)
            {
                GoToPlaylistsButton.IsVisible = true;
            }
            else if (IsSpotifyLoggedIn == false && IsYoutubeLoggedIn == false)
            {
                GoToPlaylistsButton.IsVisible = false;
            }

            if (IsSpotifyLoggedIn == true && IsYoutubeLoggedIn == true)
            {
                await Shell.Current.GoToAsync(nameof(PlaylistsPage));
            }
        }

        private void LogOutYoutube_Clicked(object sender, EventArgs e)
        {
            IsYoutubeLoggedIn = false;
            IsSpotifyLoggedIn = false;
            SecureStorage.Default.RemoveAll();
        }
    }
}