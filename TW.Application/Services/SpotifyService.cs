using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Http;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;

namespace TW.Application.Services
{
    public class SpotifyService : ISpotifyService
    {
        private readonly string _clientId = "2d84ad5eeb3f4b7c9fcc6cc479ab2d4a";
        private string _verifier;
        private string _challenge;
        private SpotifyClient _spotifyClient;
        private bool _IsLoggedIn = false;

        public SpotifyService()
        {
            var (verifier, challenge) = PKCEUtil.GenerateCodes();
            _verifier = verifier;
            _challenge = challenge;
        }

        public async Task<Uri> AuthorizeWithPKCE()
        {
            var loginRequest = new LoginRequest(
              new Uri($"https://localhost:5001/api/Spotify/callback"),
              _clientId,
              LoginRequest.ResponseType.Code
            )
            {
                CodeChallengeMethod = "S256",
                CodeChallenge = _challenge,
                Scope = new[] { Scopes.PlaylistReadPrivate, Scopes.PlaylistReadCollaborative }
            };
            var uri = loginRequest.ToUri();
            return uri;
        }

        // This method should be called from your web-server when the user visits "http://localhost:5000/api/Spotify/callback"
        public async Task GetCallback(string code)
        {
            var initialResponse = await new OAuthClient().RequestToken(
              new PKCETokenRequest(_clientId, code, new Uri("https://localhost:5001/api/Spotify/callback"), _verifier)
            );
            //Automatically refresh tokens with PKCEAuthenticator
            var authenticator = new PKCEAuthenticator(_clientId, initialResponse);

            var config = SpotifyClientConfig.CreateDefault()
              .WithAuthenticator(authenticator);

            _spotifyClient = new SpotifyClient(config);

            _IsLoggedIn = true;
        }
        public async Task<List<SimplePlaylist>> GetPlaylists()
        {
            var result = await _spotifyClient.Playlists.CurrentUsers();
            return result.Items;
        }
        public async Task<bool> IsLoggedIn()
        {
            return _IsLoggedIn;
        }
    }
}
