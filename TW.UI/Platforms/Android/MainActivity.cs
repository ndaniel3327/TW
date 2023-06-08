using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using TW.UI.Constants;
using TW.UI.Helpers;
using TW.UI.Pages;
using TW.UI.Services.Youtube;

namespace TW.UI
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]

    [IntentFilter(
    new[] { Intent.ActionView  },
    Categories = new[] { Intent.CategoryDefault ,Intent.CategoryBrowsable},
    //DataSchemes = new[] {"http","https"},
    DataSchemes = new[] { "com.googleusercontent.apps.829868223814-gn9dbtit6si40k2vd7thblkfi4a1lv4i", "com.googleusercontent.apps.829868223814-gn9dbtit6si40k2vd7thblkfi4a1lv4i:" }
       // AutoVerify =true
    )]
    public class MainActivity : MauiAppCompatActivity
    {
        public MainActivity()
        {
            
        }

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var uri = Intent?.Data;   
        
            if ( uri != null && uri.ToString().StartsWith("com.googleusercontent.apps.829868223814-gn9dbtit6si40k2vd7thblkfi4a1lv4i"))
            { 
                var uriString = uri.ToString();
                var codeIndex = uriString.IndexOf("code");
                var uriSubstring = uriString.Substring(codeIndex).Split("&");
                var code = uriSubstring[0].Substring(uriSubstring[0].IndexOf("=") + 1);

                var youtubeService = ServiceHelper.GetService<IYoutubeClientService>();

                youtubeService.GetAuthorizationToken(code);

            }
        }
    }
}