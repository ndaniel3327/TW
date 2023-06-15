using TW.UI.Pages;

namespace TW.UI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute($"{nameof(SpotifyPlaylistsPage)}", typeof(SpotifyPlaylistsPage));
            Routing.RegisterRoute($"{nameof(YoutubePlaylistsPage)}", typeof(YoutubePlaylistsPage));
        }
    }
}