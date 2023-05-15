using System.Text.Json;
using System.Text.Json.Serialization;
using TW.Infrastracture.Constants;
using TW.UI.Helpers;
using TW.UI.Services.Contracts;

namespace TW.UI.Services
{
    public class SpotifyClientService
        : ISpotifyClientService

    {
        private readonly HttpsConnectionHelper _httpsHelper;
        private readonly HttpClient _httpClient;
        //TODO: (Static or not?,constants uppercase or lowercase?) Extract all strings like "/api/Spotify" in a SpotifyConstants class
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
        public async Task<List<string>> GetPlaylists()
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync(_httpsHelper.ServerRootUrl + SpotifyConstants.PlaylistsEndpoint);
            if (responseMessage.IsSuccessStatusCode)
            {
                string content = await responseMessage.Content.ReadAsStringAsync();

                //TODO: (I tried)  Extract this code to Helpers.JsonSerializerHelper as DeserializeJson
                //var options = new JsonSerializerOptions();
                //options.PropertyNameCaseInsensitive = true;
                //options.Converters.Add(new JsonStringEnumConverter());

                //Use newly created method in Helpers.JsonSerializerHelper as JsonSerializer.DeserializeJson
                //use it like this
                //var playlits = JsonSerializer.Deserialize<List<SpotifyPlaylistContract>>(content);


                //Added by DanJR
                var playlists = JsonSerializerHelper.DeserializeJson<List<string>>(content);
                //var playlists = JsonSerializer.Deserialize<List<string>>(content);
                return playlists;
            }
            else
            {
                return null;
            }
        }
        public async Task<bool> IsLoggedIn()
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync(_httpsHelper.ServerRootUrl + SpotifyConstants.IsLoggedInEndpoint);
            if (responseMessage.IsSuccessStatusCode)
            {
                string content = await responseMessage.Content.ReadAsStringAsync();
                bool result = JsonSerializer.Deserialize<bool>(content);
                return result;
            }
            else
            {
                return false;
            }
        }
    }
}

