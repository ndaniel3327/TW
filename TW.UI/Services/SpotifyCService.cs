using System.Text.Json;
using TW.UI.Helpers;

namespace TW.UI.Services
{
    public class SpotifyCService
        : ISpotifyCService

    {
        private readonly HttpsConnectionHelper _httpsHelper;
        private readonly HttpClient _httpClient;
        //private readonly string _baseAddress;
        //private readonly string _url;

        public SpotifyCService()
        {
            _httpsHelper = new HttpsConnectionHelper(port: 5001);
            _httpClient = _httpsHelper.HttpClient;

            //_httpClient = new HttpClient();
            //_httpClient = httpClient;

            //_baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:5001" : "https://localhost:5001";

            //_url = $"{_baseAddress}/api/Spotify";

        }

        public  async Task<Uri> AuthorizeSpotify()
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
        public async void GetCallback()
        {
            await _httpClient.GetAsync(_httpsHelper.ServerRootUrl + "/api/callback");
        }
        public async Task<List<string>> GetPlaylists()
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync(_httpsHelper.ServerRootUrl + "/playlists");
            if (responseMessage.IsSuccessStatusCode)
            {
                string content = await responseMessage.Content.ReadAsStringAsync();
                List<string> playlists = JsonSerializer.Deserialize<List<string>>(content);
                return playlists;
            }
            else
            {
                return null;
            }
        }
    }
}
