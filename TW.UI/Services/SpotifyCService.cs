
using System.Diagnostics;
using System.Net.Http.Headers;

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

            _baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5000" : "https://localhost:44325";

            _url = $"{_baseAddress}/api/Spotify";

        }

        public  async void AuthorizeSpotify()
        {
            Debug.WriteLine("MAUI is Authorizing Spotify. Sending Request to Application...");
            await _httpClient.GetAsync(_url);

        }
    }
}
