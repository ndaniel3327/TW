using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using TW.Infrastracture.Constants;
using TW.UI.Constants;
using TW.UI.Helpers;
using TW.UI.Pages;
using TW.UI.Services.Spotify;
using TW.UI.Services.Youtube;

namespace TW.UI
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]

    [IntentFilter(
    new[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
    DataSchemes = new[] { "com.googleusercontent.apps.829868223814-gn9dbtit6si40k2vd7thblkfi4a1lv4i", "com.googleusercontent.apps.829868223814-gn9dbtit6si40k2vd7thblkfi4a1lv4i:"}
    // AutoVerify =true
    )]
    [IntentFilter(
    new[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
    DataSchemes = new[] { "oauth"}
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

            if (uri != null && uri.ToString().StartsWith("com.googleusercontent.apps.829868223814-gn9dbtit6si40k2vd7thblkfi4a1lv4i"))
            {
                var uriString = uri.ToString();
                var codeIndex = uriString.IndexOf("code");
                var uriSubstring = uriString.Substring(codeIndex).Split("&");
                var code = uriSubstring[0].Substring(uriSubstring[0].IndexOf("=") + 1);

                var youtubeService = ServiceHelper.GetService<IYoutubeClientService>();

                youtubeService.GetAuthorizationToken(code);

            }
            else if (uri != null && uri.ToString().StartsWith("oauth://localhost:5001/api/Spotify/callback?code"))
            {
                var _httpsHelper = new HttpsConnectionHelper(port: SpotifyConstants.HTTPSPort);
                var _httpClient = _httpsHelper.HttpClient;

                var androidurl = uri.ToString().Replace("localhost", "10.0.2.2");
                var httpsAndroidUrl = androidurl.Replace("oauth", "https");
                var result = await _httpClient.GetAsync(httpsAndroidUrl);
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var tokenDetails = JsonSerializerHelper.DeserializeJson<SpotifyTokenDetails>(content);

                    tokenDetails.SpotifyTokenExpiresInSeconds = 10;

                    tokenDetails.SpotifyTokenExpirationDate = DateTime.Now.AddSeconds(tokenDetails.SpotifyTokenExpiresInSeconds);
                    await SecureStorage.Default.SetAsync(nameof(tokenDetails.SpotifyTokenExpirationDate), tokenDetails.SpotifyTokenExpirationDate.ToString());
                    await SecureStorage.Default.SetAsync(nameof(tokenDetails.SpotifyRefreshToken), tokenDetails.SpotifyRefreshToken.ToString());

                    await Shell.Current.GoToAsync(nameof(SpotifyPlaylistsPage));
                }
            }
        }
    }
}