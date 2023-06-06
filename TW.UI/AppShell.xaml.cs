using TW.UI.Pages;

namespace TW.UI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute($"MainPage/{nameof(SpotifyPlaylistsPage)}", typeof(SpotifyPlaylistsPage));
            Routing.RegisterRoute($"MainPage/{nameof(YoutubePlaylistsPage)}", typeof(YoutubePlaylistsPage));
        }
    }
}