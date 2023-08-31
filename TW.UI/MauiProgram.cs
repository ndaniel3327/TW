using AutoMapper;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using Mopups.Hosting;
using System.Reflection;
using TW.UI.Helpers;
using TW.UI.Pages;
using TW.UI.Secrets;
using TW.UI.Services.GetDisplayedPlaylists;
using TW.UI.Services.Local;
using TW.UI.Services.SpeechToText;
using TW.UI.Services.Spotify;
using TW.UI.Services.SyncRemoteWithLocalData;
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

            builder.Services.AddTransient<ISpotifyService, SpotifyService>(serviceProvider => new SpotifyService(
                mapper: serviceProvider.GetRequiredService<IMapper>(),
                    spotifyClientId:JsonSerializerHelper.DeserializeJsonOpenAppAssetFile<AppSecret>
                    ("AppSecrets.json").SpotifyId
                ));

            builder.Services.AddTransient<IYoutubeService, YoutubeService>(
                serviceProvider => new YoutubeService(
                    youtubeClientId: JsonSerializerHelper.DeserializeJsonOpenAppAssetFile<AppSecret>
                    ("AppSecrets.json").YoutubeId
                )); 

            builder.Services.AddTransient<ILocalFilesService, LocalFilesService>();
            builder.Services.AddTransient<IRefreshLocalDataService,RefreshLocalDataService>();
            builder.Services.AddTransient<IDisplayedPlaylistsService, DisplayedPlaylistsService>();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<PlaylistsPage>();
#if ANDROID
            builder.Services.AddTransient<ISpeechToText, SpeechToText>();
#endif
            return builder.Build();
        }
    }
}