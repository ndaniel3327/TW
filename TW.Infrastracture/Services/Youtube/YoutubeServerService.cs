using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;

namespace TW.Infrastracture.Services.Youtube
{
    public class YoutubeServerService
    {
        private static string _ipAdress;
        private readonly string _clientId = "829868223814-gn9dbtit6si40k2vd7thblkfi4a1lv4i.apps.googleusercontent.com";
        private readonly string _verifier;
        private readonly string _challenge;
        private readonly HttpClient _httpClient;

        public YoutubeServerService()
        {
            var (verifier, challenge) = PKCEUtil.GenerateCodes();
            _verifier = verifier;
            _challenge = challenge;
            _httpClient = new HttpClient();
            GetLocalIPAddress();
        }
        public void GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    if (ip != null)
                        _ipAdress = ip.ToString();
                    else
                    {
                        throw new Exception("No network adapters with an IPv4 address in the system!");
                    }
                }
            }
           
        }
        public async void YoutubeAuthorization()
        {
            var ss = ".apps.googleusercontent.com";
            var ssss = "829868223814-gn9dbtit6si40k2vd7thblkfi4a1lv4i";
            var dd = "com.googleusercontent.apps.829868223814-gn9dbtit6si40k2vd7thblkfi4a1lv4i:/api/Spotify/callback";
            string myUri = "https://accounts.google.com/o/oauth2/v2/auth?" +
                "scope=https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fyoutube.readonly&" +
                "response_type=code&" +
                "state=security_token%3D138r5719ru3e1%26url%3Dhttps%3A%2F%2Foauth2.example.com%2Ftoken&" +
                //"redirect_uri=com.googleusercontent.apps.829868223814-gn9dbtit6si40k2vd7thblkfi4a1lv4i:" +
                //$"redirect_uri=https://localhost:5001/api/Spotify/callback&"+
                //$"redirect_uri=https%3A%2F%2F5001%3Alocalhost%3A%2Fapi%2FSpotify%2Fcallback&" +
                //$"redirect_uri=5001%3Alocalhost%3A%2Fapi%2FSpotify%2Fcallback" +
                "redirect_uri=com.googleusercontent.apps.829868223814-gn9dbtit6si40k2vd7thblkfi4a1lv4i%3A%2F%2Fapi%2FSpotify%2Fcallback&" +
                $"client_id={_clientId}";
            BrowserUtil.Open(new Uri(myUri));
        }
    }
}
