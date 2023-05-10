
using CommunityToolkit.Maui.Views;
using System.Diagnostics;
using TW.UI.Pages;
using TW.UI.Services;

namespace TW.UI
{
    public partial class MainPage : ContentPage
    {
        private readonly ISpotifyCService _spotifyService;

        public MainPage(ISpotifyCService spotifyService)
        {

            InitializeComponent();

            _spotifyService = spotifyService;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();

        }

        private async void OnSpotifyButtonClicked(object sender, EventArgs e)
        {
           
            Uri loginUri = await _spotifyService.AuthorizeSpotify();
            this.ShowPopup(new SpotifyAuthorizationPopup(loginUri));


        }



        private async void ContentPage_Focused(object sender, FocusEventArgs e)
        {

            if (await _spotifyService.IsLoggedIn())
            {
                await Shell.Current.GoToAsync("SpotifyPlaylists");
            }
        }

        private async void ContentPage_Loaded(object sender, EventArgs e)
        {

            if (await _spotifyService.IsLoggedIn())
            {
                await Shell.Current.GoToAsync("SpotifyPlaylists");
            }
        }
    }
}