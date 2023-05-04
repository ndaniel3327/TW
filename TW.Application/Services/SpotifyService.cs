using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TW.Application.Services
{
    public class SpotifyService : ISpotifyService
    {
        private readonly string _clientId = "2d84ad5eeb3f4b7c9fcc6cc479ab2d4a";
        private string _verifier;
        private string _challenge;
        private SpotifyClient _spotifyClient;
        public SpotifyService()
        {
            var (verifier, challenge) = PKCEUtil.GenerateCodes();
            _verifier = verifier;
            _challenge = challenge;
            Console.WriteLine(verifier);
            Console.WriteLine(challenge);
        }

        public async Task AuthorizeWithPKCE()
        {
            // Make sure "http://localhost:5000/callback" is in your applications redirect URIs!
            var loginRequest = new LoginRequest(
              new Uri("http://localhost:5000/api/Spotify/callback"),
              _clientId,
              LoginRequest.ResponseType.Code
            )
            {
                CodeChallengeMethod = "S256",
                CodeChallenge = _challenge,
                Scope = new[] { Scopes.PlaylistReadPrivate, Scopes.PlaylistReadCollaborative }
            };
            var uri = loginRequest.ToUri();
            Console.WriteLine(_challenge);
            // Redirect user to uri via your favorite web-server or open a local browser window
            BrowserUtil.Open(uri);

        }

        // This method should be called from your web-server when the user visits "http://localhost:5000/callback"
        public async Task GetCallback(string code)
        {
            Console.WriteLine(_verifier);
            var initialResponse = await new OAuthClient().RequestToken(
              new PKCETokenRequest(_clientId, code, new Uri("http://localhost:5000"), _verifier)
            );
            //Automatically refresh tokens with PKCEAuthenticator
            var authenticator = new PKCEAuthenticator(_clientId, initialResponse);

            var config = SpotifyClientConfig.CreateDefault()
              .WithAuthenticator(authenticator);

            _spotifyClient = new SpotifyClient(config);

            var playlist = _spotifyClient.Playlists;
        }

    }
}
