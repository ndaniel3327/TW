using AutoMapper;
using SpotifyAPI.Web;
using TW.Infrastracture.Constants;
using TW.UI.Models.Spotify.Data;
using TW.UI.Pages;

namespace TW.UI.Services.Spotify
{
    public class SpotifyService : ISpotifyService
    {
        private SpotifyClient _spotifyClient;
        private IPlaylistsClient _spotifyClientPlaylists => _spotifyClient.Playlists;

        private readonly string _codeChallengeMethod = "S256";
        private readonly string _clientId = "2d84ad5eeb3f4b7c9fcc6cc479ab2d4a";
        private readonly string _verifier;
        private readonly string _challenge;

        private readonly IMapper _mapper;

        public SpotifyService(IMapper mapper)
        {
            var (verifier, challenge) = PKCEUtil.GenerateCodes();
            _verifier = verifier;
            _challenge = challenge;
            _mapper = mapper;

            SetUpSpotifyClient();
            //TODO: Add Null verifier where neccesary
        }

        private async void SetUpSpotifyClient()
        {
            var tokenExpirationDate = await SecureStorage.Default.GetAsync(SpotifyConstants.StorageNameSpotifyTokenExpirationDate);
            if (tokenExpirationDate != null && DateTime.Compare(DateTime.Parse(tokenExpirationDate), DateTime.Now) > 0)
            {
                CreateClient();

            }
            else if (tokenExpirationDate != null && DateTime.Compare(DateTime.Parse(tokenExpirationDate), DateTime.Now) < 0)
            {
                var refreshToken = await SecureStorage.Default.GetAsync(SpotifyConstants.StorageNameRefreshToken);
                var isSuccess = await RefreshAccessToken(refreshToken);
                if (isSuccess == true)
                {
                    CreateClient();
                }
            }
            async void CreateClient()
            {
                var authorizationToken = await SecureStorage.Default.GetAsync(SpotifyConstants.StorageNameAccessToken);
                _spotifyClient = new SpotifyClient(authorizationToken);
            }


        }

        public async Task<Uri> StartAuthorizationWithPKCE()
        {
            //TODO Invoke an awaitable method that is showing the loading screen
            //TODO: Add Null verifier where neccesary
            var loginRequest = new LoginRequest(
              new Uri(SpotifyConstants.SpotifyCallbackAdress),
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

        public async Task ExchangeCodeForToken(string code)
        {
            //TODO: Forward exception for every possible null
            var initialResponse = await new OAuthClient().RequestToken(
              new PKCETokenRequest(_clientId, code, new Uri(SpotifyConstants.SpotifyCallbackAdress), _verifier)
            );

            var tokenExpirationDate = initialResponse.CreatedAt.AddSeconds(initialResponse.ExpiresIn);

            await SecureStorage.Default.SetAsync(SpotifyConstants.StorageNameAccessToken, initialResponse.AccessToken);
            await SecureStorage.Default.SetAsync(SpotifyConstants.StorageNameRefreshToken, initialResponse.RefreshToken);
            await SecureStorage.Default.SetAsync(SpotifyConstants.StorageNameSpotifyTokenExpirationDate, tokenExpirationDate.ToString());

            await Shell.Current.GoToAsync(nameof(SpotifyPlaylistsPage));
        }

        public async Task<List<SpotifyPlaylist>> GetPlaylists()
        {
            var result = await _spotifyClientPlaylists.CurrentUsers();

            var playlists = result.Items.Select(x => new SpotifyPlaylist()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            foreach (var playlist in playlists)
            {
                var playlistTracks = _spotifyClientPlaylists.GetItems(playlist.Id);
                var myTracks = playlistTracks.Result.Items.Select(c => c.Track);
                playlist.Tracks = _mapper.Map<List<SpotifyTrack>>(myTracks);

            }
            return playlists;
        }

        public async Task<bool> RefreshAccessToken(string refreshToken)
        {
            var newResponse = await new OAuthClient().RequestToken(new PKCETokenRefreshRequest(_clientId, refreshToken));

            if (newResponse.AccessToken != null)
            {
                var tokenExpirationDate = newResponse.CreatedAt.AddSeconds(newResponse.ExpiresIn);

                await SecureStorage.Default.SetAsync(SpotifyConstants.StorageNameAccessToken, newResponse.AccessToken);
                await SecureStorage.Default.SetAsync(SpotifyConstants.StorageNameRefreshToken, newResponse.RefreshToken);
                await SecureStorage.Default.SetAsync(SpotifyConstants.StorageNameSpotifyTokenExpirationDate, tokenExpirationDate.ToString());

                return true;
            }

            return false;

        }
    }
}
