using CommunityToolkit.Maui.Views;
using System.ComponentModel;
using TW.Infrastracture.Constants;
using TW.UI.Helpers;
using TW.UI.ViewModels;

namespace TW.UI.Pages;

public partial class SpotifyAuthorizationPopup : Popup
{
    private readonly Delegate _myDelegate;

    public SpotifyAuthorizationPopup(Uri loginUri , Delegate myDelegate)
    {
        InitializeComponent();
        BindingContext = new SpotifyAuthorizationPopupViewModel(loginUri);
        _myDelegate = myDelegate;
    }
    private void webView_Navigating(object sender, WebNavigatingEventArgs e)
    {
        //TODO: (done) refactor this to use https
        if (e.Url.StartsWith("https://localhost"))
        {
            e.Cancel = true;
            Task.Run(async () =>
            {
                var _httpsHelper = new HttpsConnectionHelper(port: SpotifyConstants.HTTPSPort);
                var _httpClient = _httpsHelper.HttpClient;

                var url = e.Url.Replace("localhost", "10.0.2.2");
                var result = await _httpClient.GetAsync(url);
                if (result.IsSuccessStatusCode)
                {
                    Close();

                }
            });
        }
    }
    private void Popup_Closed(object sender, CommunityToolkit.Maui.Core.PopupClosedEventArgs e)
    {
        Device.InvokeOnMainThreadAsync(()=>  _myDelegate.DynamicInvoke()) ;
      
    }
}