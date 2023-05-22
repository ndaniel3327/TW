using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Components.Forms;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using System;
using System.Net.Http;
using System.Threading;

namespace TW.Infrastracture.Services.Youtube
{
    public class YoutubeServerService
    {
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
        }

        public async void Authorization()
        {
            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            new ClientSecrets
            {
                ClientId = "829868223814-gn9dbtit6si40k2vd7thblkfi4a1lv4i.apps.googleusercontent.com",
            },
            new[] { YouTubeService.Scope.YoutubeReadonly },
            "user",
            CancellationToken.None,
            new FileDataStore("Youtube.ListMyPlaylists"));
        }
        public async void YoutubeAuthorization()
        {
            string myUri = "https://accounts.google.com/o/oauth2/v2/auth?" +
                "scope=https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fyoutube.readonly&" +
                "response_type=code&" +
                "state=security_token%3D138r5719ru3e1%26url%3Dhttps%3A%2F%2Foauth2.example.com%2Ftoken&" +
                "redirect_uri=com.googleusercontent.apps.829868223814-gn9dbtit6si40k2vd7thblkfi4a1lv4i:/oauth2redirect&" +
                $"client_id={_clientId}&"+
                $"code_challenge={_challenge}&"+
                "code_challenge_method=S256&";
            BrowserUtil.Open(new Uri(myUri));
        }
    }
}
