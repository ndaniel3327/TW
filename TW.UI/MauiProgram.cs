using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using System.Reflection;
using TW.UI.Constants;
using TW.UI.Pages;
using TW.UI.Services.Spotify;
using TW.UI.Services.Youtube;

namespace TW.UI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            builder.Services.AddTransient<ISpotifyService, SpotifyService>();
            builder.Services.AddSingleton<IYoutubeClientService, YoutubeClientService>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<SpotifyPlaylistsPage>(provider=>new SpotifyPlaylistsPage(provider.GetService<ISpotifyService>(),SpotifyConstants.playlistGroups));
            builder.Services.AddTransient<YoutubePlaylistsPage>();

            return builder.Build();
        }
    }
}