
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
            string[] androidUrl = new string[] { "http://10.0.2.2:5000", "http://10.0.2.2:57975", "http://10.0.2.2:5001", "http://10.0.2.2:44325" };
            //string[] localHostUrl = new string[] { "http://10.0.2.2:5000", "http://10.0.2.2:57975", "https://10.0.2.2:5001", "https://10.0.2.2:44325" }
            Debug.WriteLine("MAUI is Authorizing Spotify. Sending Request to Application...");
            
            var aaa =await _httpClient.GetAsync(_url);

        }
    }
}
