using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TW.Application.Services
{
    public class SpotifyService:ISpotifyService
    {
        private readonly string _clientId = "2d84ad5eeb3f4b7c9fcc6cc479ab2d4a";
        private readonly string _clientSecret = "7168a8fbddd94270a29a9ffdc3cdfc8e";
        public SpotifyService()
        {
            var config = SpotifyClientConfig
       .CreateDefault()
       .WithAuthenticator(new ClientCredentialsAuthenticator(_clientId, _clientSecret));

            var spotify = new SpotifyClient(config);
        }
        private EmbedIOAuthServer _server;



        public void AuthorizeAndGetToken()
        {
            var loginRequest = new LoginRequest(
                 new Uri("http://localhost:5000"),
                                _clientId,
                    LoginRequest.ResponseType.Token
)
            {
                Scope = new[] { Scopes.PlaylistReadPrivate, Scopes.PlaylistReadCollaborative }
            };
            var uri = loginRequest.ToUri();
            // Redirect user to uri via your favorite web-server
           
        }
        public async Task AuthorizeAndGetTokenOld()
        {
            // Make sure "http://localhost:5000/callback" is in your spotify application as redirect uri!

            _server = new EmbedIOAuthServer(new Uri("http://localhost:5000/callback"), 5000);
            await _server.Start();

            _server.ImplictGrantReceived += OnImplicitGrantReceived;
            _server.ErrorReceived += OnErrorReceived;

            var request = new LoginRequest(_server.BaseUri, _clientId, LoginRequest.ResponseType.Token)
            {
                Scope = new List<string> { Scopes.UserReadEmail }
            };
            BrowserUtil.Open(request.ToUri());
        }

        private async Task OnImplicitGrantReceived(object sender, ImplictGrantResponse response)
        {
            await _server.Stop();
            var spotify = new SpotifyClient(response.AccessToken);
            // do calls with Spotify
        }

        private async Task OnErrorReceived(object sender, string error, string state)
        {
            Console.WriteLine($"Aborting authorization, error received: {error}");
            await _server.Stop();
        }

    }
}
