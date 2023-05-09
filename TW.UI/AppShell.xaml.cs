using TW.UI.Pages;

namespace TW.UI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("SpotifyPlaylists", typeof(SpotifyPlaylistsPage));
        }
    }
}