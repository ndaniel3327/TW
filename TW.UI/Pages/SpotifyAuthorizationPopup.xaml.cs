using CommunityToolkit.Maui.Views;
using System.ComponentModel;
using TW.UI.ViewModels;

namespace TW.UI.Pages;

public partial class SpotifyAuthorizationPopup : Popup
{
    public SpotifyAuthorizationPopup(Uri loginUri)
    {
        InitializeComponent();
        BindingContext = new SpotifyAuthorizationPopupViewModel(loginUri);

    }

    public void WebViewSourceChanged(object sender, PropertyChangedEventArgs e)
    {
       
        //Console.WriteLine(this.webView.Source.ToString());
        //if (this.webView.Source.ToString().Contains("localhost"))
        //{
        //    string webViewUrl = this.webView.Source.ToString();
        //    webViewUrl = webViewUrl.Replace("localhost", "10.0.2.2");
        //    this.webView.Source = webViewUrl;
        //}
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
        //{
        //    string webViewUrl = this.webView.Source.ToString();
        //    webViewUrl = webViewUrl.Replace("localhost", "10.0.2.2");
        //    this.webView.Source = webViewUrl;
        //}
    }


    private void webView_BindingContextChanged(object sender, EventArgs e)
    {

    }

    private void webView_Navigated(object sender, WebNavigatedEventArgs e)
    {

    }

    private void Popup_Closed(object sender, CommunityToolkit.Maui.Core.PopupClosedEventArgs e)
    {

    }
}