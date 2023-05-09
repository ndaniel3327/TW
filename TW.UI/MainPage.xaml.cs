
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

        private async void OnSpotifyButtonClicked(object sender, EventArgs e)
        {
           
            Uri loginUri = await _spotifyService.AuthorizeSpotify();
            this.ShowPopup(new SpotifyAuthorizationPopup(loginUri));
        }
    }
}