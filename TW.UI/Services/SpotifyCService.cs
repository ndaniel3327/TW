using System.Text.Json;

namespace TW.UI.Services
{
    public class SpotifyCService
        : ISpotifyCService

    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;
        private readonly string _url;

        public SpotifyCService(HttpClient httpClient)
        {
            _httpClient = new HttpClient();
            //_httpClient = httpClient;

            _baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5000" : "https://localhost:5001";

            _url = $"{_baseAddress}/api/spotify";
        }

        public async Task<bool> AuthorizeSpotify()
        {
            await _httpClient.GetAsync(_url);
            return true;
        }
    }
}
