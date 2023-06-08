using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using TW.Infrastructure.Models;
using TW.Infrastracture.AppSettings;

namespace TW.Infrastracture.Services.Spotify
{
    public class SpotifyServerService : ISpotifyServerService
    {
       
        private bool _isLoggedIn = false;
        private SpotifyClient _spotifyClient;
        private IPlaylistsClient _spotifyClientPlaylists => _spotifyClient.Playlists;
        private readonly string _codeChallengeMethod = "S256";
        private readonly string _clientId = "2d84ad5eeb3f4b7c9fcc6cc479ab2d4a";
        private readonly string _verifier;
        private readonly string _challenge;

        private readonly IMapper _mapper;
        private readonly IAppSettings _appSettings;

        public SpotifyServerService(IMapper mapper, IAppSettings appSettings)
        {
            var (verifier, challenge) = PKCEUtil.GenerateCodes();
            _verifier = verifier;
            _challenge = challenge;
            _mapper = mapper;
            _appSettings = appSettings;
            //TODO: Add Null verifier where neccesary
        }

        public async Task<Uri> AuthorizeWithPKCE()
        {
            //TODO Invoke an awaitable method that is showing the loading screen
            //TODO: Add Null verifier where neccesary
            var loginRequest = new LoginRequest(
              new Uri(_appSettings.SpotifyCallbackEndpoint),
              _clientId,
              LoginRequest.ResponseType.Code
            )
            {
                CodeChallengeMethod = _codeChallengeMethod,
                CodeChallenge = _challenge,
                Scope = new[] { Scopes.PlaylistReadPrivate, Scopes.PlaylistReadCollaborative }
            };
            var uri = loginRequest.ToUri();
            return uri;
        }

        // This method should be called from your web-server when the user visits "http://localhost:5000/api/Spotify/callback"
        public async Task GetCallback(string code)
        {
            //TODO: Forward exception for every possible null
            var initialResponse = await new OAuthClient().RequestToken(
              new PKCETokenRequest(_clientId, code, new Uri(_appSettings.SpotifyCallbackEndpoint), _verifier)
            );

            var tResponse = new { AccessToken=initialResponse.AccessToken};

            //Automatically refresh tokens with PKCEAuthenticator
            var authenticator = new PKCEAuthenticator(_clientId, initialResponse);

            var config = SpotifyClientConfig.CreateDefault()
              .WithAuthenticator(authenticator);

            _spotifyClient = new SpotifyClient(config);
            
            _isLoggedIn = true;
        }
        public async Task<List<Playlist>> GetPlaylists()
        {
            var result = await _spotifyClientPlaylists.CurrentUsers();

            var playlists = result.Items.Select(x => new Playlist()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            foreach (var playlist in playlists)
            {
                var playlistTracks = _spotifyClientPlaylists.GetItems(playlist.Id);
                var myTracks = playlistTracks.Result.Items.Select(c => c.Track);
                playlist.Tracks = _mapper.Map<List<Track>>(myTracks);

            }
            //List<Paging<PlaylistTrack<IPlayableItem>>> listOfPagingPlaylistsWithTracks = new();
            //foreach (var playlist in result.Items)
            //{
            //    listOfPagingPlaylistsWithTracks.Add(await _spotifyClient.Playlists.GetItems(playlist.Id));
            //}

            //List<Playlist> finalPlaylist = new ();
            //foreach (var playlist in listOfPagingPlaylistsWithTracks)
            //{
            //    finalPlaylist.Where(e=>e.Tracks) = _mapper.Map<List<Track>>(playlist.Items);
            //}
            //List<Playlist> playlists = new();
            //for (int i = 0; i < finalPlaylist.Count; i++)
            //{
            //    playlists[i].Name = playlistNames[i];
            //    playlists[i].Tracks = finalPlaylist[i];
            //}

            return playlists;

        }
    }
}
