﻿using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using System.Web;
using TW.UI.Helpers;
using TW.UI.Services;

namespace TW.UI
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]

    [IntentFilter(
    new[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
    DataSchemes = new[] { "com.googleusercontent.apps.829868223814-gn9dbtit6si40k2vd7thblkfi4a1lv4i", "com.googleusercontent.apps.829868223814-gn9dbtit6si40k2vd7thblkfi4a1lv4i:"}
    )]
    [IntentFilter(
    new[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
    DataSchemes = new[] { "oauth"}
    )]
    public class MainActivity : MauiAppCompatActivity
    {
        public static Activity Instance { get; set; }

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Instance = this;

            var uri = Intent?.Data;

            if (uri != null && uri.ToString().StartsWith("com.googleusercontent.apps.829868223814-gn9dbtit6si40k2vd7thblkfi4a1lv4i"))
            {
                var uriString = uri.ToString();
                var codeIndex = uriString.IndexOf("code");
                var uriSubstring = uriString.Substring(codeIndex).Split("&");
                var code = uriSubstring[0].Substring(uriSubstring[0].IndexOf("=") + 1);

                var youtubeService = ServiceHelper.GetService<IYoutubeService>();
                await youtubeService.GetAuthorizationToken(code);

                var mainPage = ServiceHelper.GetService<MainPage>();
                mainPage.IsYoutubeLoggedIn = true;

            }
            else if (uri != null && uri.ToString().StartsWith("oauth://localhost:5001/api/Spotify/callback"))
            {
                string code = HttpUtility.ParseQueryString(uri.Query).Get("code");

                var spotifyService = ServiceHelper.GetService<ISpotifyService>();
                await spotifyService.ExchangeCodeForToken(code);

                var mainPage = ServiceHelper.GetService<MainPage>();
                mainPage.IsSpotifyLoggedIn= true;
            }
        }
    }
}