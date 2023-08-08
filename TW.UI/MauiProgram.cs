using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using Mopups.Hosting;
using System.Reflection;
using TW.UI.Pages;
using TW.UI.Services.Local;
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
                .ConfigureMopups()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

            builder.Services.AddTransient<ISpotifyService, SpotifyService>();
            builder.Services.AddTransient<IYoutubeService, YoutubeService>();
            builder.Services.AddTransient<ILocalFilesService, LocalFilesService>();

            builder.Services.AddSingleton<MainPage>();
            //builder.Services.AddTransient<YoutubePlaylistsPopup>();  ?????
            builder.Services.AddTransient<PlaylistsPage>();

            return builder.Build();
        }
    }
}