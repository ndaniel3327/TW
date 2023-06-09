using System.Text.Json;
using TW.Infrastracture.Constants;
using TW.UI.Helpers;
using TW.UI.Models;

namespace TW.UI.Services.Spotify
{
    public class SpotifyClientService
        : ISpotifyClientService

    {
        private readonly HttpsConnectionHelper _httpsHelper;
        private readonly HttpClient _httpClient;

        public SpotifyClientService()
        {
            _httpsHelper = new HttpsConnectionHelper(port: SpotifyConstants.HTTPSPort);
            _httpClient = _httpsHelper.HttpClient;
        }

        public async Task<Uri> AuthorizeSpotify()
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync(_httpsHelper.ServerRootUrl + SpotifyConstants.AuthorizationEndpoint);
            if (responseMessage.IsSuccessStatusCode)
            {
                string content = await responseMessage.Content.ReadAsStringAsync();
                Uri loginUri = JsonSerializer.Deserialize<Uri>(content);
                return loginUri;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<SpotifyPlaylistModel>> GetPlaylists()
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync(_httpsHelper.ServerRootUrl + SpotifyConstants.PlaylistsEndpoint);
            if (responseMessage.IsSuccessStatusCode)
            {
                string content = await responseMessage.Content.ReadAsStringAsync();
                var playlists = JsonSerializerHelper.DeserializeJson<List<SpotifyPlaylistModel>>(content);
                return playlists;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> RefreshAccessToken()
        {
            HttpResponseMessage responseMessage = await _httpClient.PostAsync(_httpsHelper.ServerRootUrl + SpotifyConstants.RefreshAccessTokenEndpoint, new StringContent( await SecureStorage.GetAsync("SpotifyRefreshToken")));
            if (responseMessage.IsSuccessStatusCode)
            {
                var content = await responseMessage.Content.ReadAsStringAsync();
                var tokenDetails = JsonSerializerHelper.DeserializeJson<SpotifyTokenDetails>(content);
                tokenDetails.SpotifyTokenExpiresInSeconds = 10;
                tokenDetails.SpotifyTokenExpirationDate=tokenDetails.SpotifyTokenCreatedAtDate.AddSeconds(tokenDetails.SpotifyTokenExpiresInSeconds);
                await SecureStorage.Default.SetAsync(nameof(tokenDetails.SpotifyTokenExpirationDate),
                    tokenDetails.SpotifyTokenExpirationDate.ToString());
                return true;
            }
            return false;
        }
    }
}

