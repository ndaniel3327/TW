using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using System.Reflection;
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
            builder.Services.AddSingleton<ISpotifyService, SpotifyService>();
            builder.Services.AddSingleton<IYoutubeClientService, YoutubeClientService>();
            //Main page should be singleton for the code to work
            builder.Services.AddSingleton<MainPage>();
           // builder.Services.AddTransient<SpotifyPlaylistsPage>();
            builder.Services.AddTransient<YoutubePlaylistsPage>();


            //Adding spotify service as transient will result in Invalid grant exception ( challange and verifier wont match )



            return builder.Build();
        }
    }
}