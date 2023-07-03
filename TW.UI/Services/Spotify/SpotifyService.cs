using AutoMapper;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using TW.UI.Constants;
using TW.UI.Helpers;
using TW.UI.Models.Spotify.Data;
using TW.UI.Models.Spotify.View;

namespace TW.UI.Services.Spotify
{
    public class SpotifyService : ISpotifyService
    {
        private readonly string _redirectUri = "oauth://localhost:5001/api/Spotify/callback";
        private readonly string _clientId = "2d84ad5eeb3f4b7c9fcc6cc479ab2d4a";
        private readonly string _codeChallengeMethod = "S256";

        private readonly IMapper _mapper;

        public SpotifyService(IMapper mapper)
        {
            _mapper = mapper;
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

            await SecureStorage.Default.SetAsync(SpotifyConstants.StorageNameCodeVerifier, codeVerifier);

            string myStringUri = "https://accounts.spotify.com/authorize?" +
                "response_type=code&" +
                $"client_id={_clientId}&" +
                $"scope={scope}&" +
                $"redirect_uri={_redirectUri}&" +
                $"code_challenge_method={_codeChallengeMethod}&" +
                $"code_challenge={codeChallenge}";

            Uri myUri = new Uri(myStringUri);

            return myUri;
        }
        public async Task ExchangeCodeForToken(string code)
        {
            string codeVerifier = await SecureStorage.Default.GetAsync(SpotifyConstants.StorageNameCodeVerifier);

            string myStringUri = "https://accounts.spotify.com/api/token";
            string postRequestContent =
            "grant_type=authorization_code&" +
            $"code={code}&" +
            $"redirect_uri={_redirectUri}&" +
            $"client_id={_clientId}&" +
            $"code_verifier={codeVerifier}";

            HttpClient httpClient = new HttpClient();
            var response = await httpClient.PostAsync(myStringUri, new StringContent(postRequestContent, Encoding.UTF8, "application/x-www-form-urlencoded"));
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var tokenDetails = JsonSerializerHelper.DeserializeJson<SpotifyTokenDetails>(responseContent);
                //TODO: Delete this:
                tokenDetails.SpotifyTokenExpiresInSeconds = 30;
                var tokenExpirationDate = DateTime.Now.AddSeconds(tokenDetails.SpotifyTokenExpiresInSeconds);

                await SecureStorage.Default.SetAsync(SpotifyConstants.StorageNameAccessToken, tokenDetails.SpotifyAccessToken);
                await SecureStorage.Default.SetAsync(SpotifyConstants.StorageNameRefreshToken, tokenDetails.SpotifyRefreshToken);
                await SecureStorage.Default.SetAsync(SpotifyConstants.StorageNameTokenExpirationDate, tokenExpirationDate.ToString());
                await SecureStorage.Default.SetAsync(SpotifyConstants.StorageNameTokenType, tokenDetails.SpotifyTokenType);
            }

        }
        public async Task<bool> RefreshAccessToken()
        {
            var refreshToken = await SecureStorage.Default.GetAsync(SpotifyConstants.StorageNameRefreshToken);
            string myStringUri = "https://accounts.spotify.com/api/token";
            string postRequestContent =
            "grant_type=refresh_token&" +
            $"client_id={_clientId}&" +
            $"refresh_token={refreshToken}";

            HttpClient httpClient = new HttpClient();
            var response = await httpClient.PostAsync(myStringUri, new StringContent(postRequestContent, Encoding.UTF8, "application/x-www-form-urlencoded"));

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var tokenDetails = JsonSerializerHelper.DeserializeJson<SpotifyTokenDetails>(responseContent);
                //TODO: Delete this:
                tokenDetails.SpotifyTokenExpiresInSeconds = 30;
                var tokenExpirationDate = DateTime.Now.AddSeconds(tokenDetails.SpotifyTokenExpiresInSeconds);

                await SecureStorage.Default.SetAsync(SpotifyConstants.StorageNameAccessToken, tokenDetails.SpotifyAccessToken);
                await SecureStorage.Default.SetAsync(SpotifyConstants.StorageNameRefreshToken, tokenDetails.SpotifyRefreshToken);
                await SecureStorage.Default.SetAsync(SpotifyConstants.StorageNameTokenExpirationDate, tokenExpirationDate.ToString());

                return true;
            }

            return false;
        }

        public async Task<List<SpotifyPlaylistDetails>> GetPlaylists()
        {
            string playlistsEndpoint = "https://api.spotify.com/v1/me/playlists";

            string accessToken = await SecureStorage.Default.GetAsync(SpotifyConstants.StorageNameAccessToken);
            string tokenType = await SecureStorage.Default.GetAsync(SpotifyConstants.StorageNameTokenType);

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue(tokenType, accessToken);

            var responseMessage = await httpClient.GetAsync(playlistsEndpoint);
            var content = await responseMessage.Content.ReadAsStringAsync();
            var playlistList = JsonSerializerHelper.DeserializeJson<SpotifyPlaylistList>(content);

            var playlistGroupView = new List<SpotifyPlaylistDetails>();

            foreach (var playlist in playlistList.Playlists)
            {
                var response = await httpClient.GetAsync($"https://api.spotify.com/v1/playlists/{playlist.Id}/tracks?fields=items(track(name,artists(name)))");
                var jsonContent = await response.Content.ReadAsStringAsync();
                var playlistItems = JsonSerializerHelper.DeserializeJson<SpotifyTrackList>(jsonContent);
                playlistGroupView.Add
                    (
                    new SpotifyPlaylistDetails
                    {
                        Id = playlist.Id,
                        PlaylistGroup = new SpotifyPlaylistGroup(playlist.Name, _mapper.Map<List<SpotifyTrackView>>(playlistItems.Tracks))
                    });
            }

            return playlistGroupView;
        }
    }
}
