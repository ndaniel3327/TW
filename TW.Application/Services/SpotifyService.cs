using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Http;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using TW.Application.Models;

namespace TW.Application.Services
{
    public class SpotifyService : ISpotifyService
    {
        private readonly string _clientId = "2d84ad5eeb3f4b7c9fcc6cc479ab2d4a";
        private readonly IMapper _mapper;
        private string _verifier;
        private string _challenge;
        private SpotifyClient _spotifyClient;
        private bool _IsLoggedIn = false;

        public SpotifyService(IMapper mapper)
        {
            var (verifier, challenge) = PKCEUtil.GenerateCodes();
            _verifier = verifier;
            _challenge = challenge;
            _mapper = mapper;
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
        public async Task<List<Playlist>> GetPlaylists()
        {
            var result = await _spotifyClient.Playlists.CurrentUsers();

            var playlists = result.Items.Select(x => new Playlist() 
            { 
                Id = x.Id, 
                Name = x.Name
            }).ToList<Playlist>();

            foreach (var playlist in playlists)
            {
                var playlistTracks = _spotifyClient.Playlists.GetItems(playlist.Id);
                var myTracks = playlistTracks.Result.Items;
                playlist.Tracks = _mapper.Map<List<Track>>(playlistTracks.Result.Items);

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
        public async Task<bool> IsLoggedIn()
        {
            return await Task.Run(() => _IsLoggedIn);
        }
    }
}
