using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace TW.UI
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]

    [IntentFilter(new[] {Intent.ActionView},
        Categories = new[]
        {
            Intent.ActionView,
            Intent.CategoryDefault,
            Intent.CategoryBrowsable
        },
        DataScheme ="",
        DataHost = "com.googleusercontent.apps.829868223814-gn9dbtit6si40k2vd7thblkfi4a1lv4i:",
        AutoVerify =true
        )
        ]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var uri = Intent?.Data;
            if (uri != null)
            {
                //Wont work
                string token = uri.ToString().Substring(uri.ToString().LastIndexOf('=')+1);
            }
        }
        protected override void OnResume()
        {
            base.OnResume();
            Platform.OnResume(this);
        }
        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            var action = intent.Action;
            var strLink = intent.DataString;
            if(Intent.ActionView==action && !string.IsNullOrWhiteSpace(strLink))
            {
                string token = strLink.Substring(strLink.LastIndexOf('=') + 1);
            }

            //2nd way 
            var data = intent.DataString;

            if (intent.Action != Intent.ActionView) return;
            if (string.IsNullOrWhiteSpace(data)) return;

            if (data.Contains("/oauth"))
            {
                //do what you want 
            }
        }
    }
}