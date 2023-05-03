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

        private void OnSpotifyButtonClicked(object sender, EventArgs e)
        {
            _spotifyService.AuthorizeSpotify();
        }
    }
}