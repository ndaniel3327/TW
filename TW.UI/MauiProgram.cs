using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TW.UI.Pages;
using TW.UI.Services;
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
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddScoped<ISpotifyClientService, SpotifyClientService>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<SpotifyPlaylistsPage>();
            //builder.Services.AddSingleton<SpotifyAuthorizationPopup>();
            //builder.Services.AddSingleton<SpotifyAuthorizationPopupViewModel>();

            return builder.Build();
        }
    }
}