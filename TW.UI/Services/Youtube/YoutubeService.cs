using System.Net.Http.Headers;
using System.Text;
using TW.UI.Constants;
using TW.UI.Enums;
using TW.UI.Helpers;
using TW.UI.Models;
using TW.UI.Models.Youtube.Data;
using TW.UI.Pages;

namespace TW.UI.Services.Youtube
{
    public class YoutubeService
        : IYoutubeService
    {
        private readonly string _clientId = "829868223814-gn9dbtit6si40k2vd7thblkfi4a1lv4i.apps.googleusercontent.com";

        public Uri GetAuthorizationLink()
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
            var response = await httpClient.PostAsync(myUri, new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded"));

            var responseJson = await response.Content.ReadAsStringAsync();
            var token = JsonSerializerHelper.DeserializeJson<YoutubeTokenDetails>(responseJson);
            //TODO: Remove this
            token.YoutubeExpiresInSeconds = 10;
            var addingDate = DateTime.Now;
            var expirationDate = addingDate.AddSeconds(token.YoutubeExpiresInSeconds);

            await SecureStorage.Default.SetAsync(YoutubeConstants.StorageNameAccessToken, token.YoutubeAccessToken);
            await SecureStorage.Default.SetAsync(YoutubeConstants.StorageNameRefreshToken, token.YoutubeRefreshToken);
            await SecureStorage.Default.SetAsync(YoutubeConstants.StorageNameTokenType, token.YoutubeTokenType);
            await SecureStorage.Default.SetAsync(YoutubeConstants.StorageNameTokenExpirationDate, expirationDate.ToString());
        }

        public async Task<List<PlaylistDisplayGroup>> GetYoutubePlaylists()
        {
            string accessToken = await SecureStorage.Default.GetAsync("YoutubeAccessToken");
            string tokenType = await SecureStorage.Default.GetAsync("YoutubeTokenType");

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue(tokenType, accessToken);

            var responseMessage = await httpClient.GetAsync("https://www.googleapis.com/youtube/v3/playlists?" +
                "part=snippet&" +
                "mine=true");
            var listOfYoutubePlaylists = await responseMessage.Content.ReadAsStringAsync();
            var playlists = JsonSerializerHelper.DeserializeJson<YoutubePlaylistList>(listOfYoutubePlaylists);

            foreach (var playlist in playlists.Playlists)
            {
                var localresponseMessage = await httpClient.GetAsync("https://www.googleapis.com/youtube/v3/playlistItems?" +
                "part=snippet&" +
                $"playlistId={playlist.Id}");

                var youtubePlaylistItems = await localresponseMessage.Content.ReadAsStringAsync();
                var deserializedPlaylist = JsonSerializerHelper.DeserializeJson<YoutubePlaylist>(youtubePlaylistItems);

                playlist.Tracks = deserializedPlaylist.Tracks;
            }

            var playlistsModel = new List<PlaylistDisplayGroup>();
            foreach (var playlist in playlists.Playlists)
            {
                var tracks = new List<PlaylistDisplayTracks>();
                var trackNameList = playlist.Tracks.Select(q => q.TrackInfo.Name);
                foreach (var trackName in trackNameList)
                {
                    tracks.Add(new PlaylistDisplayTracks() { Name = trackName });
                }
                var playlistModel = new PlaylistDisplayGroup(playlist.Id, playlist.PlaylistInfo.Name, tracks,PlaylistSource.Youtube, ImageSource.FromFile("youtubeicon.svg"));
                playlistsModel.Add(playlistModel);
            }
            return playlistsModel;
        }

        public async Task<bool> RefreshAccessToken()
        {
            string refreshToken = await SecureStorage.Default.GetAsync("YoutubeRefreshToken");

            var httpClient = new HttpClient();

            string myUri = "https://oauth2.googleapis.com/token";
            string content =
                $"client_id={_clientId}&" +
                "grant_type=refresh_token&" +
                $"refresh_token={refreshToken}";

            var response = await httpClient.PostAsync(myUri, new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded"));
            if(response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var token = JsonSerializerHelper.DeserializeJson<YoutubeTokenDetails>(responseJson);
                //TODO: Remove this
                token.YoutubeExpiresInSeconds = 10;
                var addingDate = DateTime.Now;
                var expirationDate = addingDate.AddSeconds(token.YoutubeExpiresInSeconds);

                await SecureStorage.Default.SetAsync(YoutubeConstants.StorageNameAccessToken, token.YoutubeAccessToken);
                await SecureStorage.Default.SetAsync(YoutubeConstants.StorageNameTokenType, token.YoutubeTokenType);
                await SecureStorage.Default.SetAsync(YoutubeConstants.StorageNameTokenExpirationDate , expirationDate.ToString());

                return true;
            }
            return false;
        }

    }
}
