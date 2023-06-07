using System.Net.Http.Headers;
using System.Text;
using TW.UI.Helpers;
using TW.UI.Pages;

namespace TW.UI.Services.Youtube
{
    public class YoutubeClientService : IYoutubeClientService
    {   
        private readonly string _clientId = "829868223814-gn9dbtit6si40k2vd7thblkfi4a1lv4i.apps.googleusercontent.com";

        public Uri AuthorizeYoutube()
        {


            string myUri = "https://accounts.google.com/o/oauth2/v2/auth?" +
              "scope=https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fyoutube.readonly&" +
              "response_type=code&" +
              "state=security_token%3D138r5719ru3e1%26url%3Dhttps%3A%2F%2Foauth2.example.com%2Ftoken&" +
              "redirect_uri=com.googleusercontent.apps.829868223814-gn9dbtit6si40k2vd7thblkfi4a1lv4i:&" +
              $"client_id={_clientId}";
            
            return new Uri(myUri);
        }
        public async void GetAuthorizationToken(string authorizationCode)
        {

            string myUri = "https://oauth2.googleapis.com/token";
            var content = $"client_id={_clientId}&" +
                $"code={authorizationCode}&" +
                "grant_type=authorization_code&" +
                "redirect_uri=com.googleusercontent.apps.829868223814-gn9dbtit6si40k2vd7thblkfi4a1lv4i:";

            HttpClient httpClient = new HttpClient();
            var response =  await httpClient.PostAsync(myUri, new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded"));

            var responseJson = await response.Content.ReadAsStringAsync();
            var token = JsonSerializerHelper.DeserializeJson<YoutubeAccessToken>(responseJson);
            token.AddingDate= DateTime.Now;
            token.ExpirationDate = token.AddingDate.AddSeconds(token.ExpiresInSeconds);

            await SecureStorage.Default.SetAsync(nameof(token.AccessToken),token.AccessToken );
            await SecureStorage.Default.SetAsync(nameof(token.RefreshToken), token.RefreshToken);
            await SecureStorage.Default.SetAsync(nameof(token.TokenType), token.TokenType);
            await SecureStorage.Default.SetAsync(nameof(token.ExpiresInSeconds), token.ExpiresInSeconds.ToString());
            await SecureStorage.Default.SetAsync(nameof(token.AddingDate),token.AddingDate.ToString());
            await SecureStorage.Default.SetAsync(nameof(token.ExpirationDate), token.ExpirationDate.ToString());


            await Shell.Current.GoToAsync(nameof(YoutubePlaylistsPage));
        }

        public async Task<YoutubePlaylistGroup> GetYoutubePlaylists()
        {
            string accessToken = await SecureStorage.Default.GetAsync("AccessToken");
            string tokenType = await SecureStorage.Default.GetAsync("TokenType");

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue(tokenType, accessToken);

            var responseMessage = await httpClient.GetAsync("https://www.googleapis.com/youtube/v3/playlists?" +
                "part=snippet&" +
                "mine=true");
            var youtubePlaylists = await responseMessage.Content.ReadAsStringAsync();
            var playlists = JsonSerializerHelper.DeserializeJson<YoutubePlaylistGroup>(youtubePlaylists);

            var playlistGroup = new YoutubePlaylistGroup();
            foreach (var playlist in playlists.Playlists)
            {
                var localresponseMessage = await httpClient.GetAsync("https://www.googleapis.com/youtube/v3/playlistItems?" +
                "part=snippet&" +
                $"playlistId={playlist.Id}");

                var youtubePlaylistItems = await localresponseMessage.Content.ReadAsStringAsync();
                var deserializedPlaylist = JsonSerializerHelper.DeserializeJson<YoutubePlaylist>(youtubePlaylistItems);
                //PlaylistGroup.Playlists.Add(deserializedPlaylist);
                playlist.Tracks= deserializedPlaylist.Tracks;
            }
            return playlists;
        }
        //public async void GetYoutubePlaylistItems(HttpClient httpClient,YoutubePlaylist playlist,YoutubePlaylistGroup playlistGroup)
        //{
            



        //}

    }
}
