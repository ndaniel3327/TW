using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TW.UI.Pages;
using TW.UI.Services.Spotify;
using TW.UI.Services.Youtube;
using TW.UI.ViewModels;

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
                //.ConfigureEssentials(e =>
                //{
                //    e.AddAppAction(new AppAction("id1", "Messages", icon: "messages"));
                //})
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<ISpotifyClientService, SpotifyClientService>();
            builder.Services.AddSingleton<IYoutubeClientService, YoutubeClientService>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<SpotifyPlaylistsPage>();
            builder.Services.AddSingleton<YoutubePlaylistsPage>();

            //builder.Services.AddSingleton<SpotifyAuthorizationPopup>();
            //builder.Services.AddSingleton<SpotifyAuthorizationPopupViewModel>();

            return builder.Build();
        }
    }
}