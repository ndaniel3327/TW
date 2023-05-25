using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views.Animations;
using Android.Widget;

namespace TW.UI
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]

    //[IntentFilter(new[] {Intent.ActionView},
    //    Categories = new[]
    //    {
    //        Intent.ActionView,
    //        Intent.CategoryDefault,
    //        Intent.CategoryBrowsable
    //    },
    //    DataScheme ="",
    //    DataHost = "com.googleusercontent.apps.829868223814-gn9dbtit6si40k2vd7thblkfi4a1lv4i:",
    //    AutoVerify =true
    //    )
    //    ]
    [IntentFilter(
    new[] { Intent.ActionView  },
    Categories = new[] { Intent.CategoryDefault ,Intent.CategoryBrowsable},
    //DataSchemes = new[] {"http","https"},
    DataSchemes = new[] { "com.googleusercontent.apps.829868223814-gn9dbtit6si40k2vd7thblkfi4a1lv4i", "com.googleusercontent.apps.829868223814-gn9dbtit6si40k2vd7thblkfi4a1lv4i:" }
       // AutoVerify =true
    )]
    //[IntentFilter(
    //new[] { Intent.ActionView },
    //Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
    //DataSchemes = new[] { "http", "https" },
    //DataHosts = new[] { "oauth2.googleapis.com" }
    //)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var uri = Intent?.Data;
            if ( uri != null )
            {
                string apitoken = uri.ToString().Substring(uri.ToString().IndexOf('=') + 1);    
            }
        }
        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            var action = intent.Action;
            var strLink = Intent.DataString;
            if(Intent.ActionView==action && !string.IsNullOrWhiteSpace(strLink))
            {
                string apitoken = strLink.Substring(strLink.IndexOf("=")+1);
            }
        }

    }
    
}