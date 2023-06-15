using CommunityToolkit.Maui.Views;
using System.ComponentModel;
using TW.Infrastracture.Constants;
using TW.UI.Helpers;
using TW.UI.Services.Spotify;
using TW.UI.ViewModels;

namespace TW.UI.Pages;

public partial class SpotifyAuthorizationPopup : Popup
{
    private readonly Delegate _myDelegate;

    public SpotifyAuthorizationPopup(Uri loginUri, Delegate myDelegate)
    {
        InitializeComponent();
        BindingContext = new SpotifyAuthorizationPopupViewModel(loginUri);
        _myDelegate = myDelegate;
    }
    private async void webView_Navigating(object sender, WebNavigatingEventArgs e)
    {
        if (e.Url.StartsWith("oauth://localhost"))
        {
            e.Cancel = true;
            await Task.Run(async () =>
            {
                var _httpsHelper = new HttpsConnectionHelper(port: SpotifyConstants.HTTPSPort);
                var _httpClient = _httpsHelper.HttpClient;

                var androidUrl = e.Url.Replace("localhost", "10.0.2.2");
                var httpsAndroidUrl = androidUrl.Replace("oauth", "https");
                var result = await _httpClient.GetAsync(httpsAndroidUrl);
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    // var tokenDetails = JsonSerializerHelper.DeserializeJson<SpotifyTokenDetails>(content);

                    //tokenDetails.SpotifyTokenExpiresInSeconds = 10;

                    //tokenDetails.SpotifyTokenExpirationDate = DateTime.Now.AddSeconds(tokenDetails.SpotifyTokenExpiresInSeconds);
                    //await SecureStorage.Default.SetAsync(nameof(tokenDetails.SpotifyTokenExpirationDate), tokenDetails.SpotifyTokenExpirationDate.ToString());
                    //await SecureStorage.Default.SetAsync(nameof(tokenDetails.SpotifyRefreshToken), tokenDetails.SpotifyRefreshToken.ToString());
                    //Close();
                }
            });
        }
        else if (e.Url.StartsWith("https://accounts.spotify.com/en/login/google?"))
        {
            e.Cancel = true;
            await Browser.OpenAsync(e.Url);
        }
    }
    private void Popup_Closed(object sender, CommunityToolkit.Maui.Core.PopupClosedEventArgs e)
    {
        //Device.InvokeOnMainThreadAsync(() => _myDelegate.DynamicInvoke());

    }
}