using TW.UI.Pages;

namespace TW.UI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(PlaylistsPage),typeof(PlaylistsPage));
        }
    }
}