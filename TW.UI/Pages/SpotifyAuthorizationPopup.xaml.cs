using CommunityToolkit.Maui.Views;
using System.ComponentModel;
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
        //Console.WriteLine(this.webView.Source.ToString());
        if (e.Url.StartsWith("https://localhost"))
        {
            e.Cancel = true;
            Task.Run(async () =>
            {
                HttpClient hc = new HttpClient();

                var url = e.Url.Replace("localhost", "10.0.2.2").Replace("https", "http").Replace("5001", "5000");
                var sd = await hc.GetAsync(url);
                if(sd.IsSuccessStatusCode)
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