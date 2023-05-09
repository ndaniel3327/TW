
using System.Diagnostics;
using System.Text.Json;

namespace TW.UI.Services
{
    public class SpotifyCService
        : ISpotifyCService

    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;
        private readonly string _url;

        public SpotifyCService()
        {
            _httpClient = new HttpClient();
            //_httpClient = httpClient;

            _baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5000" : "https://localhost:5001";

            _url = $"{_baseAddress}/api/Spotify";

        }

        public  async Task<Uri> AuthorizeSpotify()
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync(_url);
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
            HttpResponseMessage responseMessage = await _httpClient.GetAsync(_url+"/playlists");
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
