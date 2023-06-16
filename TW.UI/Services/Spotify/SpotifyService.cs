using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using TW.Infrastracture.Constants;
using TW.UI.Helpers;
using TW.UI.Models.Spotify.Data;

namespace TW.UI.Services.Spotify
{
    public class SpotifyService : ISpotifyService
    {
        private readonly string _clientId = "2d84ad5eeb3f4b7c9fcc6cc479ab2d4a";
        private readonly string _codeChallengeMethod = "S256";
        public SpotifyService()
        {

        }
        private string GenerateCodeVerifier(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var charArray = new char[length];
            for (int i = 0; i < charArray.Length; i++)
            {
                charArray[i] = chars[random.Next(chars.Length)];
            }

            return new string(charArray);
        }
        private string GenerateCodeChallange(string codeVerifier)
        {
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
            var b64Hash = Convert.ToBase64String(hash);
            var code = Regex.Replace(b64Hash, "\\+", "-");
            code = Regex.Replace(code, "\\/", "_");
            code = Regex.Replace(code, "=+$", "");

            return code;
        }
        public async Task<Uri> GetAuthorizationUri()
        {
            string scope = "playlist-read-private playlist-read-collaborative";
            string codeVerifier = GenerateCodeVerifier(100);
            string codeChallenge = GenerateCodeChallange(codeVerifier);

            await SecureStorage.Default.SetAsync(SpotifyConstants.StorageNameSpotifyCodeVerifier, codeVerifier);

            string myStringUri = "https://accounts.spotify.com/authorize?" +
                "response_type=code&" +
                $"client_id={_clientId}&" +
                $"scope={scope}&" +
                $"redirect_uri={SpotifyConstants.SpotifyRedirectUri}&" +
                $"code_challenge_method={_codeChallengeMethod}&" +
                $"code_challenge={codeChallenge}";

            Uri myUri = new Uri(myStringUri);

            return myUri;
        }
        public async Task ExchangeCodeForToken(string code)
        {
            string codeVerifier = await SecureStorage.Default.GetAsync(SpotifyConstants.StorageNameSpotifyCodeVerifier);

            string myStringUri = "https://accounts.spotify.com/api/token?";
            string postRequestContent =
            "grant_type=authorization_code&" +
            $"code={code}&" +
            $"redirect_uri={SpotifyConstants.SpotifyRedirectUri}&" +
            $"client_id={_clientId}&" +
            $"code_verifier={codeVerifier}";

            //TODO:move redirect uri in this class as private field;

            HttpClient httpClient = new HttpClient();
            var response = await httpClient.PostAsync(myStringUri, new StringContent(postRequestContent, Encoding.UTF8, "application/x-www-form-urlencoded"));
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var tokenDetails = JsonSerializerHelper.DeserializeJson<SpotifyTokenDetails>(responseContent);
            }

        }

        //    private SpotifyClient _spotifyClient;
        //    private IPlaylistsClient _spotifyClientPlaylists => _spotifyClient.Playlists;

        //    private readonly string _codeChallengeMethod = "S256";
        //    private readonly string _clientId = "2d84ad5eeb3f4b7c9fcc6cc479ab2d4a";
        //    private readonly string _verifier;
        //    private readonly string _challenge;

        //    private readonly IMapper _mapper;

        //    public SpotifyService(IMapper mapper)
        //    {
        //        var (verifier, challenge) = PKCEUtil.GenerateCodes();
        //        _verifier = verifier;
        //        _challenge = challenge;
        //        _mapper = mapper;
        //        //TODO: Add Null verifier where neccesary
        //    }

        //    private async Task SetUpSpotifyClient()
        //    {

        //        StringBuilder sb = new StringBuilder();
        //        sb.AppendLine();
        //        sb.AppendLine(DateTime.Now.ToString());

        //        var authorizationToken = await SecureStorage.Default.GetAsync(SpotifyConstants.StorageNameAccessToken);
        //        sb.AppendLine(authorizationToken);

        //        //File.WriteAllText(@"C:\Users\zamfi\Desktop\myLog.txt", sb.ToString());

        //        var tokenExpirationDate = await SecureStorage.Default.GetAsync(SpotifyConstants.StorageNameSpotifyTokenExpirationDate);
        //        sb.AppendLine(tokenExpirationDate);

        //        //File.WriteAllText(@"C:\Users\zamfi\Desktop\myLog.txt", sb.ToString());


        //        if (SpotifyTokenDetails.SpotifyAccessTokenExpirationDate != null && DateTime.Compare(DateTime.Parse(SpotifyTokenDetails.SpotifyAccessTokenExpirationDate), DateTime.UtcNow) < 0)
        //        {
        //            CreateClient();

        //        }
        //        else if (SpotifyTokenDetails.SpotifyAccessTokenExpirationDate != null && DateTime.Compare(DateTime.Parse(SpotifyTokenDetails.SpotifyAccessTokenExpirationDate), DateTime.UtcNow) > 0)
        //        {
        //            var refreshToken = await SecureStorage.Default.GetAsync(SpotifyConstants.StorageNameRefreshToken);
        //            var isSuccess = await RefreshAccessToken(refreshToken);
        //            if (isSuccess == true)
        //            {
        //                CreateClient();
        //            }
        //        }

        //        void CreateClient()
        //        {
        //            try
        //            {
        //                _spotifyClient = new SpotifyClient(SpotifyTokenDetails.SpotifyAccessToken);
        //            }
        //            catch (Exception ex)
        //            {
        //                Debug.WriteLine("////////////// " + ex.Message);
        //            }
        //        }
        //    }

        //    public async Task<Uri> StartAuthorizationWithPKCE()
        //    {
        //        var uri = await Task.Run(() =>
        //        {
        //            var loginRequest = new LoginRequest(
        //                new Uri(SpotifyConstants.SpotifyCallbackAdress),
        //                _clientId,
        //                LoginRequest.ResponseType.Code)
        //            {
        //                CodeChallengeMethod = _codeChallengeMethod,
        //                CodeChallenge = _challenge,
        //                Scope = new[] { Scopes.PlaylistReadPrivate, Scopes.PlaylistReadCollaborative }
        //            };
        //            var uri = loginRequest.ToUri();
        //            return uri;
        //        });
        //        return uri;
        //        //TODO Invoke an awaitable method that is showing the loading screen
        //        //TODO: Add Null verifier where neccesary
        //    }

        //    public async Task ExchangeCodeForToken(string code)
        //    {
        //        //TODO: Forward exception for every possible null
        //        var initialResponse = await new OAuthClient().RequestToken(
        //          new PKCETokenRequest(_clientId, code, new Uri(SpotifyConstants.SpotifyCallbackAdress), _verifier)
        //        );
        //        var addedDate = initialResponse.CreatedAt;
        //        var tokenExpirationDate = addedDate.AddSeconds(initialResponse.ExpiresIn);

        //        await SecureStorage.Default.SetAsync(SpotifyConstants.StorageNameAccessToken, initialResponse.AccessToken);
        //        await SecureStorage.Default.SetAsync(SpotifyConstants.StorageNameRefreshToken, initialResponse.RefreshToken);
        //        await SecureStorage.Default.SetAsync(SpotifyConstants.StorageNameSpotifyTokenExpirationDate, tokenExpirationDate.ToString());

        //        await Shell.Current.GoToAsync(nameof(SpotifyPlaylistsPage));
        //    }

        //    public async Task<List<SpotifyPlaylist>> GetPlaylists()
        //    {
        //        try
        //        {
        //            await SetUpSpotifyClient();
        //        }
        //        catch (Exception ex)
        //        {
        //            Debug.WriteLine("/////////// " + ex.Message);
        //        }

        //        var result = await _spotifyClientPlaylists.CurrentUsers();

        //        var playlists = result.Items.Select(x => new SpotifyPlaylist()
        //        {
        //            Id = x.Id,
        //            Name = x.Name
        //        }).ToList();

        //        foreach (var playlist in playlists)
        //        {
        //            var playlistTracks = _spotifyClientPlaylists.GetItems(playlist.Id);
        //            var myTracks = playlistTracks.Result.Items.Select(c => c.Track);
        //            playlist.Tracks = _mapper.Map<List<SpotifyTrack>>(myTracks);

        //        }
        //        return playlists;
        //    }

        //    public async Task<bool> RefreshAccessToken(string refreshToken)
        //    {
        //        try
        //        {
        //            var newResponse = await new OAuthClient().RequestToken(new PKCETokenRefreshRequest(_clientId, refreshToken));
        //            if (newResponse.AccessToken != null)
        //            {
        //                var tokenExpirationDate = newResponse.CreatedAt.AddSeconds(newResponse.ExpiresIn);

        //                await SecureStorage.Default.SetAsync(SpotifyConstants.StorageNameAccessToken, newResponse.AccessToken);
        //                await SecureStorage.Default.SetAsync(SpotifyConstants.StorageNameRefreshToken, newResponse.RefreshToken);
        //                await SecureStorage.Default.SetAsync(SpotifyConstants.StorageNameSpotifyTokenExpirationDate, tokenExpirationDate.ToString());

        //                return true;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Debug.WriteLine("////////////////////// " + ex.Message);
        //            return false;
        //        }
        //        return false;
        //    }

        public Task<List<SpotifyPlaylist>> GetPlaylists()
        {
            throw new NotImplementedException();
        }

        public Task<bool> RefreshAccessToken(string refreshToken)
        {
            throw new NotImplementedException();
        }

    }
}
