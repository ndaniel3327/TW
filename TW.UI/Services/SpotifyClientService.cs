using System.Text.Json;
using System.Text.Json.Serialization;
using TW.UI.Helpers;
using TW.UI.Services.Contracts;

namespace TW.UI.Services
{
    public class SpotifyClientService
        : ISpotifyClientService

    {
        private readonly HttpsConnectionHelper _httpsHelper;
        private readonly HttpClient _httpClient;
        //private readonly string _baseAddress;
        //private readonly string _url;

        //TODO: Extract all strings like "/api/Spotify" in a SpotifyConstants class

        public SpotifyClientService()
        {
            _httpsHelper = new HttpsConnectionHelper(port: 5001);//TODO: Port to SpotifyConstants
            _httpClient = _httpsHelper.HttpClient;

            //_httpClient = new HttpClient();
            //_httpClient = httpClient;

            //_baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:5001" : "https://localhost:5001";

            //_url = $"{_baseAddress}/api/Spotify";

        }



        public async Task<Uri> AuthorizeSpotify()
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync(_httpsHelper.ServerRootUrl + "/api/Spotify");
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
            HttpResponseMessage responseMessage = await _httpClient.GetAsync(_httpsHelper.ServerRootUrl + "/api/Spotify/playlists");
            if (responseMessage.IsSuccessStatusCode)
            {
                string content = await responseMessage.Content.ReadAsStringAsync();

                //TODO:  Extract this code to Helpers.JsonSerializerHelper as DeserializeJson
                //var options = new JsonSerializerOptions();
                //options.PropertyNameCaseInsensitive = true;
                //options.Converters.Add(new JsonStringEnumConverter());

                //TODO: use newly created method in Helpers.JsonSerializerHelper as JsonSerializer.DeserializeJson
                //use it like this
                //var playlits = JsonSerializer.Deserialize<List<SpotifyPlaylistContract>>(content);
                var playlists = JsonSerializer.Deserialize<List<string>>(content);
                return playlists;
            }
            else
            {
                return null;
            }
        }
        public async Task<bool> IsLoggedIn()
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync(_httpsHelper.ServerRootUrl + "/api/Spotify/IsLoggedIn");
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

